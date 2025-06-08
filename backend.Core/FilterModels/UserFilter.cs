namespace backend.Core.Models.FilterModels;

public record UserFilter
{
    public int? Year { get; set; }
    public string? FirstName { get; set; } 
    public string? LastName { get; set; }
}