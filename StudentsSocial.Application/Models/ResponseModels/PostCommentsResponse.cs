using StudentsSocial.Core.Entities;

namespace StudentsSocial.Application.Models.ResponseModels;

public record PostCommentsResponse
{
    public Guid PostId { get; set; }
    public List<CommentEntity> Content { get; set; } = null!;
    public int TotalCount { get; set; }
}