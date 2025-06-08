using backend.Core.Entities;

namespace backend.Application.ResponseModels;

public record PostsResponse
{
    public List<PostEntity> Content { get; set; } = null!;
    public int TotalCount { get; set; }
}