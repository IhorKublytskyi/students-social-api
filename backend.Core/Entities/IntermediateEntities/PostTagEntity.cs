namespace backend.Core.Entities;

public class PostTagEntity
{
    public Guid PostId { get; set; }
    public PostEntity Post { get; set; }
    public Guid UserId { get; set; }
    public UserEntity User { get; set; }
    public DateTime TaggedAt { get; set; }
}