using backend.Core.Entities;

namespace backend.Application.Models.ResponseModels;

public record PostsResponse
{
    public List<PostEntity> Content { get; set; } = null!;
    public int TotalCount { get; set; }
}