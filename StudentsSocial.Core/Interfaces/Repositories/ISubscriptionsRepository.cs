using StudentsSocial.Core.Entities;

namespace StudentsSocial.Core.Interfaces.Repositories;

public interface ISubscriptionsRepository
{
    Task AddAsync(SubscriptionEntity subscription);
    Task<bool> DeleteAsync(Guid subscriberId, Guid subscribedId);
    Task<bool> ExistsAsync(Guid subscriberId, Guid subscribedId);
    Task<SubscriptionEntity?> GetAsync(Guid subscriberId, Guid subscribedId);
}