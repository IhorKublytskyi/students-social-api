namespace backend.Application.Models.ResponseModels;

public record UserResponse
{
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public byte[] ProfilePicture { get; set; } = new byte[0];
    public string Status { get; set; } = string.Empty;
    public DateTime BirthDate { get; set; }
    public string Biography { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public int FollowersCount { get; set; }
    public int FollowedCount { get; set; }
    public int PostsCounts { get; set; }
}