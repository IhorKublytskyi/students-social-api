namespace backend.Core.Entities;

public class ReplyEntity
{
    public Guid RepliedId { get; set; }
    public CommentEntity Replied { get; set; }
    public Guid ReplyId { get; set; }
    public CommentEntity Reply { get; set; }
    public DateTime RepliedAt { get; set; }
}