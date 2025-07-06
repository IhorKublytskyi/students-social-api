using StudentsSocial.Core.Entities;

namespace StudentsSocial.Core.Interfaces.Repositories;

public interface IRefreshTokensRepository
{
    Task AddAsync(RefreshTokenEntity refreshToken);
    Task DeleteAsync(Guid id);
    Task<RefreshTokenEntity> GetAsync(string token);
}