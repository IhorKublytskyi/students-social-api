using backend.Core.Entities;

namespace backend.Application.Models.ResponseModels;

public record PostCommentsResponse
{
    public Guid PostId { get; set; }
    public List<CommentEntity> Content { get; set; } = null!;
    public int TotalCount { get; set; }
}