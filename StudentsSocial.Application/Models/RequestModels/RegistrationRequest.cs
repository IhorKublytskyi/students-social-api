namespace StudentsSocial.Application.Models.RequestModels;

public record RegistrationRequest
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Username { get; set; }
    public string? Email { get; set; }
    public string? BirthDate { get; set; }
    public string? Password { get; set; }
}