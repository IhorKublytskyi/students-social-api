namespace backend.Core.Entities;

public class LikePostEntity
{
    public Guid UserId { get; set; }
    public UserEntity User { get; set; }
    public Guid PostId { get; set; }
    public PostEntity Post { get; set; }
    public DateTime LikedAt { get; set; }
}