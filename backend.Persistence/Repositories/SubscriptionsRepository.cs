using backend.Core.Entities;
using backend.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;

public class SubscriptionsRepository : ISubscriptionsRepository
{
    private readonly StudentsSocialDbContext _dbContext;

    public SubscriptionsRepository(StudentsSocialDbContext _dbContext)
    {
        this._dbContext = _dbContext;
    }

    public async Task<SubscriptionEntity?> Get(Guid subscriberId, Guid subscribedId)
    {
        return await _dbContext.Subscriptions
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.SubscriberId == subscriberId && s.SubscribedToId == subscribedId);
    }

    public async Task Add(SubscriptionEntity subscription)
    {
        await _dbContext.Subscriptions.AddAsync(subscription);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<bool> Delete(Guid subscriberId, Guid subscribedId)
    {
        int rowsDeleted = await _dbContext.Subscriptions
             .Where(s => s.SubscriberId == subscriberId && s.SubscribedToId == subscribedId)
             .ExecuteDeleteAsync();

        return rowsDeleted > 0;
    }

    public async Task<bool> Exists(Guid subscriberId, Guid subscribedId)
    {
        return await _dbContext.Subscriptions
            .AsNoTracking()
            .AnyAsync(s =>
            s.SubscriberId == subscriberId && s.SubscribedToId == subscribedId);
    }
}