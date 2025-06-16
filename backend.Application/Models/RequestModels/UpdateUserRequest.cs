namespace backend.Application.Models.RequestModels;

public class UpdateUserRequest
{
    public string Email { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public byte[] ProfilePicture { get; set; } = new byte[0];
    public string Status { get; set; } = string.Empty;
    public string BirthDate { get; set; } = string.Empty;
    public string Biography { get; set; } = string.Empty;
}