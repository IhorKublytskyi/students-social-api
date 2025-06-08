namespace backend.Application.RequestModels;

public record RegistrationRequest
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string BirthDate { get; set; }
    public string Password { get; set; } = string.Empty;
}