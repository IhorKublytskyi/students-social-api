using backend.Core.Entities;
using backend.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace backend.Persistence.Repositories;

public class PostsRepository : IPostsRepository
{
    private readonly StudentsSocialDbContext _dbContext;

    public PostsRepository(StudentsSocialDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Add(PostEntity post)
    {
        await _dbContext.Posts.AddAsync(post);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<List<PostEntity>> Get()
    {
        return await _dbContext.Posts
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<List<PostEntity>> GetByUserId(Guid? userId)
    {
        return await _dbContext.Posts
            .Where(p => p.UserId == userId)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<List<PostEntity>> GetByTitle(string title)
    {
        return await _dbContext.Posts
            .Where(p => p.Title == title)
            .AsNoTracking()
            .ToListAsync();
    }
}