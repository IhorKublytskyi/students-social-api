using backend.Core.Entities;
using backend.Core.Interfaces;
using backend.Infrastructure.Exceptions;
using DataAccess.Postgres;
using DataAccess.Postgres.Repositories;
using Microsoft.EntityFrameworkCore;

namespace backend.Infrastructure;
public class SubscriptionService : ISubscriptionService
{
    private readonly SubscriptionsRepository _subscriptionsRepository;
    private readonly UsersRepository _usersRepository;

    public SubscriptionService(SubscriptionsRepository subscriptionsRepository, UsersRepository usersRepository)
    {
        _subscriptionsRepository = subscriptionsRepository;
        _usersRepository = usersRepository;
    }

    private async Task<string> HandleSubscription(string subscriberUsername, string subscribedUsername, bool isSubscribing)
    {
        if (subscriberUsername == null)
            throw new UserNotFoundException("Subscriber`s name was null");

        var subscriber = await _usersRepository.GetByUsername(subscriberUsername);
        if (subscriber == null)
            throw new UserNotFoundException("Subscriber was not found");

        var subscribed = await _usersRepository.GetByUsername(subscribedUsername);
        if (subscribed == null)
            throw new UserNotFoundException("Subscribed user was not found");

        if (isSubscribing)
        {
            if (await _subscriptionsRepository.Exists(subscriber.Id, subscribed.Id))
                throw new UserAlreadyExistsException($"You are already subscribed to {subscribed.Username}");
                    
            var subscription = new SubscriptionEntity()
            {
                SubscriberId = subscriber.Id,
                SubscribedToId = subscribed.Id,
                SubscribedAt = DateTime.UtcNow
            };

            await _subscriptionsRepository.Add(subscription);

            return $"You have successfully subscribed to {subscribed.Username}";
        }
        else
        {
            bool isUnsubscribed = await _subscriptionsRepository.Delete(subscriber.Id, subscribed.Id);

            return isUnsubscribed
                ? $"You have successfully stopped following {subscribed.Username}"
                : throw new SubscriptionNotExistException($"You are not subscribed to {subscribed.Username}");
        }
    }

    public async Task<string> SubscribeAsync(string subscriberUsername, string subscribedUsername)
    {
        return await HandleSubscription(subscriberUsername, subscribedUsername, isSubscribing: true);
    }

    public async Task<string> UnsubscribeAsync(string subscriberUsername, string subscribedUsername)
    {
        return await HandleSubscription(subscriberUsername, subscribedUsername, isSubscribing: false);
    }
}