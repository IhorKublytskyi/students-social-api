using backend.Core.Entities;

namespace backend.Application.Models.ResponseModels;

public record CommentsResponse
{
    public List<CommentEntity> Content { get; set; } = null!;
    public int TotalCount { get; set; }
}