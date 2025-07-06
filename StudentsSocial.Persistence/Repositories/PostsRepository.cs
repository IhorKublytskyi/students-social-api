using StudentsSocial.Core.Entities;
using StudentsSocial.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace StudentsSocial.Persistence.Repositories;

public class PostsRepository : IPostsRepository
{
    private readonly StudentsSocialDbContext _dbContext;

    public PostsRepository(StudentsSocialDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(PostEntity post)
    {
        await _dbContext.Posts.AddAsync(post);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<List<PostEntity>> GetAsync()
    {
        return await _dbContext.Posts
            .AsNoTracking()
            .Select(post => new PostEntity() 
            {
                Id = post.Id,
                CreatedAt = post.CreatedAt,
                Title = post.Title,
                Description = post.Description,
                IsPrivate = post.IsPrivate
            }) 
            .ToListAsync();
    }

    public async Task<List<PostEntity>> GetByUserIdAsync(Guid? userId)
    {
        return await _dbContext.Posts
            .AsNoTracking()
            .Where(p => p.UserId == userId)
            .ToListAsync();
    }

    public async Task<List<PostEntity>> GetByTitleAsync(string title)
    {
        return await _dbContext.Posts
            .AsNoTracking()
            .Where(p => p.Title == title)      
            .ToListAsync();
    }

    public async Task<PostEntity?> GetByIdAsync(Guid id)
    {
        return await _dbContext.Posts
            .AsNoTracking()
            .Include(p => p.Comments)
            .FirstOrDefaultAsync(p => p.Id == id);
    }
}