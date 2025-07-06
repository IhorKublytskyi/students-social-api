using StudentsSocial.Application.Models.ResponseModels;
using StudentsSocial.Core.Results;

namespace StudentsSocial.Application.Interfaces;

public interface IRefreshTokenService
{
    Task<Result<TokensResponse>> RefreshAsync(string refreshTokenValue);
}