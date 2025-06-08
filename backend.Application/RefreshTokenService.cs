using backend.Application.Interfaces;
using backend.Core.Entities;
using backend.Core.Results;
using backend.Infrastructure;
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

    public async Task<Result<(string, string)>> RefreshAsync(string refreshTokenValue)
    {
        var refreshToken = await _refreshTokensRepository.Get(refreshTokenValue);
        if (refreshToken == null)
            return Result<(string, string)>.Failure("Refresh token not found");
        if (refreshToken.ExpireIn < DateTime.UtcNow)
            return Result<(string, string)>.Failure("Refresh token has expired");

        var user = refreshToken.User;

        await _refreshTokensRepository.Delete(refreshToken.Id);

        refreshToken = new RefreshTokenEntity
        {
            Id = Guid.NewGuid(),
            ExpireIn = DateTime.UtcNow.AddHours(_options.RefreshTokenValidityMins),
            Token = _tokenProvider.GenerateRefreshToken(),
            UserId = user.Id
        };
        await _refreshTokensRepository.Add(refreshToken);

        var accessToken = _tokenProvider.Generate(user);

        return Result<(string, string)>.Success((accessToken, refreshToken.Token));
    }
}