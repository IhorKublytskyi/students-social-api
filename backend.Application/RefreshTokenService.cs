using backend.Application.Interfaces;
using backend.Application.Models.ResponseModels;
using backend.Core.Entities;
using backend.Core.Results;
using Microsoft.Extensions.Options;
using backend.Core.Interfaces.Repositories;

namespace backend.Application;

public class RefreshTokenService : IRefreshTokenService
{
    private readonly JwtOptions _options;
    private readonly IRefreshTokensRepository _refreshTokensRepository;
    private readonly ITokenProvider _tokenProvider;

    public RefreshTokenService(
        IRefreshTokensRepository refreshTokensRepository,
        ITokenProvider tokenProvider,
        IOptions<JwtOptions> options)
    {
        _refreshTokensRepository = refreshTokensRepository;
        _tokenProvider = tokenProvider;
        _options = options.Value;
    }

    public async Task<Result<TokensResponse>> RefreshAsync(string refreshTokenValue)
    {
        var refreshToken = await _refreshTokensRepository.Get(refreshTokenValue);
        if (refreshToken == null)
            return Result<TokensResponse>.Failure("Refresh token not found");
        var utcnow = DateTime.UtcNow;
        if (refreshToken.ExpireIn < DateTime.UtcNow)
            return Result<TokensResponse>.Failure("Refresh token has expired");

        var user = refreshToken.User;

        await _refreshTokensRepository.Delete(refreshToken.Id);

        refreshToken = new RefreshTokenEntity
        {
            Id = Guid.NewGuid(),
            ExpireIn = DateTime.UtcNow.AddHours(_options.RefreshTokenValidityHours),
            Token = _tokenProvider.GenerateRefreshToken(),
            UserId = user.Id
        };
        await _refreshTokensRepository.Add(refreshToken);

        var accessToken = _tokenProvider.Generate(user);

        var response = new TokensResponse()
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken.Token
        };
        return Result<TokensResponse>.Success(response);
    }
}