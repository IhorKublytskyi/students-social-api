using StudentsSocial.Core.Entities;
using StudentsSocial.Core.Models.FilterModels;

namespace StudentsSocial.Core.Interfaces.Repositories;

public interface IUsersRepository
{
    Task AddAsync(UserEntity user);
    Task<List<UserEntity>> GetAsync();
    Task<UserEntity?> GetByEmailAsync(string email);
    Task<UserEntity?> GetByIdAsync(Guid id);
    Task<UserEntity?> GetByUsernameAsync(string username);
    Task<List<UserEntity>> GetByFilterAsync(UserFilter filter);
    Task UpdateAsync(Guid id, UserEntity user);
}