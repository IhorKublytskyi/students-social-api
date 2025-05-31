namespace backend.Core.Entities;

public class CommentEntity
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public UserEntity User { get; set; }
    public Guid PostId { get; set; }
    public PostEntity Post { get; set; }
    public string Body { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public List<LikeCommentEntity>? Likes { get; set; }
    public List<ReplyEntity>? Replies { get; set; }
}