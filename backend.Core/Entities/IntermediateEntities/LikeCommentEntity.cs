namespace backend.Core.Entities;

public class LikeCommentEntity
{
    public Guid UserId { get; set; }
    public UserEntity User { get; set; }
    public Guid CommentId { get; set; }
    public CommentEntity Comment { get; set; }
    public DateTime LikedAt { get; set; }
}