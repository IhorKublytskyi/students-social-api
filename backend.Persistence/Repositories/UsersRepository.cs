using backend.Core.Entities;
using backend.Core.Interfaces.Repositories;
using backend.Core.Models.FilterModels;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace backend.Persistence.Repositories;

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
            .Include(p => p.Posts)
            .Include(p => p.Followers)
            .Include(p => p.FollowedUsers)
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

    public async Task<List<UserEntity>> GetByFilter(UserFilter filter)
    {
        var query = _dbContext.Users
            .Include(p => p.Posts)
            .Include(p => p.Followers)
            .Include(p => p.FollowedUsers)
            .AsQueryable();

        if (filter.Year != null)
        {
            query = query.Where(u => u.BirthDate.Year == filter.Year);
        }

        if (!string.IsNullOrWhiteSpace(filter.FirstName))
        {
            query = query.Where(u => u.FirstName == filter.FirstName);
        }

        if (!string.IsNullOrWhiteSpace(filter.LastName))
        {
            query = query.Where(u => u.LastName == filter.LastName);
        }

        return await query.ToListAsync();
    }

    public async Task Update(Guid id, string email, string username, string firstName, string lastName, byte[]profilePicture, string status, DateTime birthDate, string biography)
    {
        await _dbContext.Users
            .Where(u => u.Id == id)
            .ExecuteUpdateAsync(u => u
                .SetProperty(p => p.Email, email)
                .SetProperty(p => p.Username, username)
                .SetProperty(p => p.FirstName, firstName)
                .SetProperty(p => p.LastName, lastName)
                .SetProperty(p => p.Status, status)
                .SetProperty(p => p.BirthDate, birthDate)
                .SetProperty(p => p.Biography, biography));
    }
}