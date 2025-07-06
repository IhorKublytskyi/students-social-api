namespace StudentsSocial.Application.Models.RequestModels;

public record CreatePostRequest
{
    public Guid UserId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsPrivate { get; set; }
}