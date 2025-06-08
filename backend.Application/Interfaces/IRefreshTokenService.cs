using backend.Core.Results;

namespace backend.Application.Interfaces;

public interface IRefreshTokenService
{
    Task<Result<(string, string)>> RefreshAsync(string refreshTokenValue);
}