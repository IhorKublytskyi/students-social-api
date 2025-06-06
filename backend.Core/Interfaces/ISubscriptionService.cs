using backend.Core.Results;

namespace backend.Core.Interfaces;

public interface ISubscriptionService
{
    Task<Result> SubscribeAsync(string subscriberUsername, string subscribedUsername);
    Task<Result> UnsubscribeAsync(string subscriberUsername, string subscribedUsername);
}