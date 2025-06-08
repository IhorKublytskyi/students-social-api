using backend.Application.ResponseModels;
using backend.Core.Results;

namespace backend.Application.Interfaces;

public interface IRefreshTokenService
{
    Task<Result<TokensResponse>> RefreshAsync(string refreshTokenValue);
}