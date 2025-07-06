using StudentsSocial.Core.Entities;

namespace StudentsSocial.Application.Models.ResponseModels;

public record PostsResponse
{
    public List<PostEntity> Content { get; set; } = null!;
    public int TotalCount { get; set; }
}