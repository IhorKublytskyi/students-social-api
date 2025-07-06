using StudentsSocial.Core.Entities;
using StudentsSocial.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace StudentsSocial.Persistence.Repositories;

public class RefreshTokensRepository : IRefreshTokensRepository
{
    private readonly StudentsSocialDbContext _dbContext;

    public RefreshTokensRepository(StudentsSocialDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(RefreshTokenEntity refreshToken)
    {
        await _dbContext.RefreshTokens.AddAsync(refreshToken);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<RefreshTokenEntity?> GetAsync(string token)
    {
        return await _dbContext.RefreshTokens
            .AsNoTracking()
            .Include(r => r.User)
            .FirstOrDefaultAsync(r => r.Token == token);
    }

    public async Task DeleteAsync(Guid id)
    {
        await _dbContext.RefreshTokens
            .Where(r => r.Id == id)
            .ExecuteDeleteAsync();
    }
}