namespace backend.Core.Entities;

public class SubscriptionEntity
{
    public Guid SubscriberId { get; set; }
    public UserEntity Subscriber { get; set; }
    public Guid SubscribedToId { get; set; }
    public UserEntity SubscribedTo { get; set; }
    public DateTime SubscribedAt { get; set; }
}