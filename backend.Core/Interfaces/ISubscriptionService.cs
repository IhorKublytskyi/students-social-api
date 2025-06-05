namespace backend.Core.Interfaces;

public interface ISubscriptionService
{
    Task<IAsyncResult> SubscribeAsync(string subscriber, string subscribed);
    Task<IAsyncResult> UnsubscribeAsync(string subscriber, string subscribed);
}