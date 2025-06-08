using backend.Core.Entities;

namespace backend.Application.ResponseModels;

public record CommentsResponse
{
    public List<CommentEntity> Content { get; set; } = null!;
    public int TotalCount { get; set; }
}