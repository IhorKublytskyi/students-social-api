using backend.Core.Entities;

namespace backend.Core.Interfaces.Repositories;

public interface ICommentsRepository
{
    Task Add(CommentEntity comment);
    Task<List<CommentEntity>> Get();
    Task<List<CommentEntity>> GetByPostId(Guid postId);
}