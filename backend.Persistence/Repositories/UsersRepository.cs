using backend.Core.Interfaces.Repositories;
using backend.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;

public class UsersRepository : IUsersRepository
{
    private readonly StudentsSocialDbContext _dbContext;

    public UsersRepository(StudentsSocialDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Add(UserEntity user)
    {
        await _dbContext.Users.AddAsync(user);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<List<UserEntity>> Get()
    {
        return await _dbContext.Users.AsNoTracking().ToListAsync();
    }

    public async Task<UserEntity?> GetById(Guid id)
    {
        return await _dbContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == id);
    }
    public async Task<UserEntity?> GetByEmail(string email)
    {
        return await _dbContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<UserEntity?> GetByUsername(string username)
    {
        return await _dbContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Username == username);
    }
}