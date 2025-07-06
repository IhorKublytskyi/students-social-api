namespace StudentsSocial.Application.Models.RequestModels;

public record LoginRequest
{
    public string? Email { get; set; }
    public string? Password { get; set; }
}