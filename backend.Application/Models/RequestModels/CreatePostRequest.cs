namespace backend.API.RequestModels;

public class CreatePostRequest
{
    public string UserEmail { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; }  = string.Empty;
    public bool IsPrivate { get; set; }
}