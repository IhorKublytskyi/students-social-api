namespace backend.Core.Interfaces;

public interface ISubscriptionService
{
    Task<string> SubscribeAsync(string subscriberUsername, string subscribedUsername);
    Task<string> UnsubscribeAsync(string subscriberUsername, string subscribedUsername);
}