using StudentsSocial.Core.Entities;

namespace StudentsSocial.Core.Interfaces.Repositories;

public interface IPostsRepository
{
    Task AddAsync(PostEntity post);
    Task<List<PostEntity>> GetAsync();
    Task<List<PostEntity>> GetByTitleAsync(string title);
    Task<List<PostEntity>> GetByUserIdAsync(Guid? userId);
    Task<PostEntity?> GetByIdAsync(Guid id);
}