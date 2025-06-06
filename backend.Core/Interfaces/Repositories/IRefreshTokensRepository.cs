using backend.Core.Entities;

namespace backend.Core.Interfaces.Repositories;

public interface IRefreshTokensRepository
{
    Task Add(RefreshTokenEntity refreshToken);
    Task Delete(Guid id);
    Task<RefreshTokenEntity> Get(string token);
}