using backend.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Postgres.Repositories;

public class RefreshTokensRepository
{
    private readonly StudentsSocialDbContext _dbContext;

    public RefreshTokensRepository(StudentsSocialDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Add(RefreshTokenEntity refreshToken)
    {
        await _dbContext.RefreshTokens.AddAsync(refreshToken);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<RefreshTokenEntity> Get(string token)
    {
        return await _dbContext.RefreshTokens
            .AsNoTracking()
            .Include(r => r.User)
            .FirstOrDefaultAsync(r => r.Token == token);
    }

    public async Task Delete(Guid id)
    {
        await _dbContext.RefreshTokens
            .Where(r => r.Id == id)
            .ExecuteDeleteAsync();
    }
}