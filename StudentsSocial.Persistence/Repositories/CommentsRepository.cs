using StudentsSocial.Core.Entities;
using StudentsSocial.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace StudentsSocial.Persistence.Repositories;

public class CommentsRepository : ICommentsRepository
{
    private readonly StudentsSocialDbContext _dbContext;

    public CommentsRepository(StudentsSocialDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<CommentEntity>> GetAsync()
    {
        return await _dbContext.Comments
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<List<CommentEntity>> GetByIdAsync(Guid postId)
    {
        return await _dbContext.Comments
            .AsNoTracking()
            .Where(c => c.PostId == postId)
            .ToListAsync();
    }

    public async Task AddAsync(CommentEntity comment)
    {
        await _dbContext.Comments.AddAsync(comment);
        await _dbContext.SaveChangesAsync();
    }
}