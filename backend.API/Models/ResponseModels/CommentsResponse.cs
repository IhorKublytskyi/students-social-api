namespace backend.API.ResponseModels;

using backend.Core.Entities;

public class CommentsResponse
{
    public List<CommentEntity> Content { get; set; } = null!;
    public int TotalCount { get; set; }
}