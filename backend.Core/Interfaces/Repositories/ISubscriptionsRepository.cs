using backend.Core.Entities;

namespace backend.Core.Interfaces.Repositories;
public interface ISubscriptionsRepository
{
    Task Add(SubscriptionEntity subscription);
    Task<bool> Delete(Guid subscriberId, Guid subscribedId);
    Task<bool> Exists(Guid subscriberId, Guid subscribedId);
    Task<SubscriptionEntity?> Get(Guid subscriberId, Guid subscribedId);
}