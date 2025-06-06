using backend.Core.Entities;

namespace backend.API.ResponseModels;

public class PostsResponse
{
    public List<PostEntity> Content { get; set; } = null!;
    public int TotalCount { get; set; }
}