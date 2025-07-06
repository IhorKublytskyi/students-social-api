using StudentsSocial.Core.Entities;
using StudentsSocial.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace StudentsSocial.Persistence.Repositories;

public class SubscriptionsRepository : ISubscriptionsRepository
{
    private readonly StudentsSocialDbContext _dbContext;

    public SubscriptionsRepository(StudentsSocialDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<SubscriptionEntity?> GetAsync(Guid subscriberId, Guid subscribedId)
    {
        return await _dbContext.Subscriptions
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.SubscriberId == subscriberId && s.SubscribedToId == subscribedId);
    }

    public async Task AddAsync(SubscriptionEntity subscription)
    {
        await _dbContext.Subscriptions.AddAsync(subscription);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<bool> DeleteAsync(Guid subscriberId, Guid subscribedId)
    {
        var rowsDeleted = await _dbContext.Subscriptions
            .Where(s => s.SubscriberId == subscriberId && s.SubscribedToId == subscribedId)
            .ExecuteDeleteAsync();

        return rowsDeleted > 0;
    }

    public async Task<bool> ExistsAsync(Guid subscriberId, Guid subscribedId)
    {
        return await _dbContext.Subscriptions
            .AsNoTracking()
            .AnyAsync(s =>
                s.SubscriberId == subscriberId && s.SubscribedToId == subscribedId);
    }
}