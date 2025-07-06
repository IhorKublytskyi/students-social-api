using StudentsSocial.Core.Results;

namespace StudentsSocial.Application.Interfaces;

public interface ISubscriptionService
{
    Task<Result> SubscribeAsync(string subscriberUsername, string subscribedUsername);
    Task<Result> UnsubscribeAsync(string subscriberUsername, string subscribedUsername);
    Task<Result<bool>> CheckSubscriptionAsync(string subscriberUsername, string subscribedUsername);
}