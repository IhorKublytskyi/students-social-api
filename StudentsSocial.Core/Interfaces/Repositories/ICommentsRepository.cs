using StudentsSocial.Core.Entities;

namespace StudentsSocial.Core.Interfaces.Repositories;

public interface ICommentsRepository
{
    Task AddAsync(CommentEntity comment);
    Task<List<CommentEntity>> GetAsync();
    Task<List<CommentEntity>> GetByIdAsync(Guid postId);
}