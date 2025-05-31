namespace backend.Core.Entities;

public class UserEntity
{
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string ProfilePicture { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime BirthDate { get; set; }
    public string Biography { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public bool Online { get; set; }
    public List<PostEntity>? Posts { get; set; }
    public List<CommentEntity>? Comments { get; set; }
    public List<SubscriptionEntity> Subscriptions { get; set; }
    public List<CloseFriendEntity>? ClosedFriends{ get; set; }
    public List<FavouritePostEntity>? FavouritePosts { get; set; }
    public List<LikePostEntity>? Likes { get; set; }
    public List<LikeCommentEntity> LikesComments { get; set; }
    public List<PostTagEntity> Tags { get; set; }
}