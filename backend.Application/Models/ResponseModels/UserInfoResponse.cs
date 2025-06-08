namespace backend.Application.ResponseModels;

public record UserInfoResponse
{
    public string Email { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string ProfilePicture { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime BirthDate { get; set; }
    public string Biography { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public int FollowersCount { get; set; }
    public int FollowedCount { get; set; }
    public int PostsCounts { get; set; }
}