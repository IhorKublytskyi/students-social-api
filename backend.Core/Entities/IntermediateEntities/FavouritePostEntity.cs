namespace backend.Core.Entities;

public class FavouritePostEntity
{
    public Guid UserId { get; set; }
    public UserEntity User { get; set; }
    public Guid PostId { get; set; }
    public PostEntity Post { get; set; }
    public DateTime FavouredAt { get; set; }
}