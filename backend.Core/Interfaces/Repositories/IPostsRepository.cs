using backend.Core.Entities;

namespace backend.Core.Interfaces.Repositories;

public interface IPostsRepository
{
    Task Add(PostEntity post);
    Task<List<PostEntity>> Get();
    Task<List<PostEntity>> GetByTitle(string title);
    Task<List<PostEntity>> GetByUserId(Guid? userId);
    Task<PostEntity> GetById(Guid id);
}