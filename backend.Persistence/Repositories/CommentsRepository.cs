using backend.Core.Entities;
using backend.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace backend.Persistence.Repositories;

public class CommentsRepository : ICommentsRepository
{
    private readonly StudentsSocialDbContext _dbContext;

    public CommentsRepository(StudentsSocialDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<CommentEntity>> Get()
    {
        return await _dbContext.Comments
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<List<CommentEntity>> GetByPostId(Guid postId)
    {
        return await _dbContext.Comments
            .AsNoTracking()
            .Where(c => c.PostId == postId)
            .ToListAsync();
    }

    public async Task Add(CommentEntity comment)
    {
        await _dbContext.Comments.AddAsync(comment);
        await _dbContext.SaveChangesAsync();
    }
}