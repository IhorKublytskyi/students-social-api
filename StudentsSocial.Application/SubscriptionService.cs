using StudentsSocial.Application.Interfaces;
using StudentsSocial.Core.Entities;
using StudentsSocial.Core.Interfaces.Repositories;
using StudentsSocial.Core.Results;

namespace StudentsSocial.Application;

public class SubscriptionService : ISubscriptionService
{
    private readonly ISubscriptionsRepository _subscriptionsRepository;
    private readonly IUsersRepository _usersRepository;

    public SubscriptionService(ISubscriptionsRepository subscriptionsRepository, IUsersRepository usersRepository)
    {
        _subscriptionsRepository = subscriptionsRepository;
        _usersRepository = usersRepository;
    }

    private async Task<Result> HandleSubscription(string subscriberUsername, string subscribedUsername,
        bool isSubscribing)
    {
        var subscriber = await _usersRepository.GetByUsernameAsync(subscriberUsername);
        if (subscriber == null)
            return Result.Failure("Subscriber not found");

        var subscribed = await _usersRepository.GetByUsernameAsync(subscribedUsername);
        if (subscribed == null)
            return Result.Failure("User to subscribe not found");

        if (isSubscribing)
        {
            if (await _subscriptionsRepository.ExistsAsync(subscriber.Id, subscribed.Id))
                return Result.Failure("Already subscribed");

            var subscription = new SubscriptionEntity
            {
                SubscriberId = subscriber.Id,
                SubscribedToId = subscribed.Id,
                SubscribedAt = DateTime.UtcNow
            };

            await _subscriptionsRepository.AddAsync(subscription);

            return Result.Success();
        }

        bool isUnsubscribed = await _subscriptionsRepository.DeleteAsync(subscriber.Id, subscribed.Id);

        return isUnsubscribed
            ? Result.Success()
            : Result.Failure("Not subscribed yet");
    }

    public async Task<Result<bool>> CheckSubscriptionAsync(string subscriberUsername, string subscribedUsername)
    {
        var subscriber = await _usersRepository.GetByUsernameAsync(subscriberUsername);
        if (subscriber == null)
            return Result<bool>.Failure("Subscriber not found");

        var subscribed = await _usersRepository.GetByUsernameAsync(subscribedUsername);
        if (subscribed == null)
            return Result<bool>.Failure("User to subscribe not found");

        var subscription = await _subscriptionsRepository.GetAsync(subscriber.Id, subscribed.Id);
        if (subscription == null)
            return Result<bool>.Success(false);

        return Result<bool>.Success(true);
    }

    public async Task<Result> SubscribeAsync(string subscriberUsername, string subscribedUsername)
    {
        return await HandleSubscription(subscriberUsername, subscribedUsername, true);
    }

    public async Task<Result> UnsubscribeAsync(string subscriberUsername, string subscribedUsername)
    {
        return await HandleSubscription(subscriberUsername, subscribedUsername, false);
    }
}