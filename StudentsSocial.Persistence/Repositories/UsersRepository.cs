using StudentsSocial.Core.Entities;
using StudentsSocial.Core.Interfaces.Repositories;
using StudentsSocial.Core.Models.FilterModels;
using Microsoft.EntityFrameworkCore;

namespace StudentsSocial.Persistence.Repositories;

public class UsersRepository : IUsersRepository
{
    private readonly StudentsSocialDbContext _dbContext;

    public UsersRepository(StudentsSocialDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(UserEntity user)
    {
        await _dbContext.Users.AddAsync(user);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<List<UserEntity>> GetAsync()
    {
        return await _dbContext.Users
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<UserEntity?> GetByIdAsync(Guid id)
    {
        return await _dbContext.Users
            .AsNoTracking()
            .Include(user => user.Posts)
            .Include(user => user.Followers)
            .Include(user => user.FollowedUsers)
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<UserEntity?> GetByEmailAsync(string email)
    {
        return await _dbContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<UserEntity?> GetByUsernameAsync(string username)
    {
        return await _dbContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Username == username);
    }

    public async Task<List<UserEntity>> GetByFilterAsync(UserFilter filter)
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

    public async Task UpdateAsync(Guid id, UserEntity user)
    {
        await _dbContext.Users
            .Where(u => u.Id == user.Id)
            .ExecuteUpdateAsync(u => u
                .SetProperty(p => p.Email, user.Email)
                .SetProperty(p => p.Username, user.Username)
                .SetProperty(p => p.FirstName, user.FirstName)
                .SetProperty(p => p.LastName, user.LastName)
                .SetProperty(p => p.Status, user.Status)
                .SetProperty(p => p.BirthDate, user.BirthDate)
                .SetProperty(p => p.Biography, user.Biography));
    }
}