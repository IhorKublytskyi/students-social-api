using backend.Core.Entities;
using backend.Core.Models.FilterModels;

namespace backend.Core.Interfaces.Repositories;

public interface IUsersRepository
{
    Task Add(UserEntity user);
    Task<List<UserEntity>> Get();
    Task<UserEntity?> GetByEmail(string email);
    Task<UserEntity?> GetById(Guid id);
    Task<UserEntity?> GetByUsername(string username);
    Task<List<UserEntity>> GetByFilter(UserFilter filter);
}