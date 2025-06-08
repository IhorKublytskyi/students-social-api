using backend.Core.Results;

namespace backend.Application.Interfaces;

public interface ISubscriptionService
{
    Task<Result> SubscribeAsync(string subscriberUsername, string subscribedUsername);
    Task<Result> UnsubscribeAsync(string subscriberUsername, string subscribedUsername);
    Task<Result<bool>> CheckSubscriptionAsync(string subscriberUsername, string subscribedUsername);
}