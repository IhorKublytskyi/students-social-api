using backend.Core.Interfaces;
using DataAccess.Postgres;
using DataAccess.Postgres.Repositories;

namespace backend.Infrastructure;

public class SubscriptionService : ISubscriptionService
{
    private readonly UsersRepository _usersRepository;
    private readonly StudentsSocialDbContext _db;

    public SubscriptionService(
        UsersRepository usersRepository, 
        StudentsSocialDbContext db
        )
    {
        _usersRepository = usersRepository;
        _db = db;
    }
    private Task<IAsyncResult> HandleSubscription(string subscriberUsername, string subscribedUsername, bool isSubscribing)
    {
        if (subscribedUsername == null)
            return null;

        return null;
    }

    public Task<IAsyncResult> SubscribeAsync(string subscriberUsername, string subscribedUsername)
    {
        HandleSubscription(subscriberUsername, subscribedUsername, isSubscribing: true);
        return null;
    }

    public Task<IAsyncResult> UnsubscribeAsync(string subscriberUsername, string subscribedUsername)
    {
        HandleSubscription(subscriberUsername, subscribedUsername, isSubscribing: false);
        return null;
    }
}