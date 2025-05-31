namespace backend.Core.Entities;

public class CloseFriendEntity
{
    public Guid OwnerUserId { get; set; }
    public UserEntity OwnerUser { get; set; }
    public Guid CloseFriendUserId { get; set; }
    public UserEntity CloseFriendUser { get; set; }
    public DateTime FollowedAt { get; set; }
}