namespace backend.Core.Entities;

public class RefreshTokenEntity
{
    public Guid Id { get; set; }
    public string Token { get; set; }
    public DateTime ExpireIn { get; set; }
    public Guid UserId { get; set; }
    public UserEntity User { get; set; }
}