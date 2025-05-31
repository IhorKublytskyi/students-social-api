namespace backend.Core.Entities;

public class PostEntity
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public UserEntity User { get; set; }
    public DateTime CreatedAt { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsPrivate { get; set; }

    public List<VideoEntity>? Videos { get; set; }
    public List<ImageEntity>? Images { get; set; }
    public List<CommentEntity>? Comments { get; set; }
    public List<FavouritePostEntity>? FavouredBy { get; set; }
    public List<PostTagEntity>? Tags { get; set; }
    public List<LikePostEntity>? Likes { get; set; }
}