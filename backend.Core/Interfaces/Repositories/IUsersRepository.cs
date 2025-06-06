using backend.Core.Entities;

namespace backend.Core.Interfaces.Repositories;

public interface IUsersRepository
{
    Task Add(UserEntity user);
    Task<List<UserEntity>> Get();
    Task<UserEntity?> GetByEmail(string email);
    Task<UserEntity?> GetById(Guid id);
    Task<UserEntity?> GetByUsername(string username);
}