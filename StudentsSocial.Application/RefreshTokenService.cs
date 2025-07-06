using StudentsSocial.Application.Interfaces;
using StudentsSocial.Application.Models.ResponseModels;
using StudentsSocial.Core.Entities;
using StudentsSocial.Core.Results;
using Microsoft.Extensions.Options;
using StudentsSocial.Core.Interfaces.Repositories;

namespace StudentsSocial.Application;

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
        var refreshToken = await _refreshTokensRepository.GetAsync(refreshTokenValue);
        if (refreshToken == null)
            return Result<TokensResponse>.Failure("Refresh token not found");
        var utcnow = DateTime.UtcNow;
        if (refreshToken.ExpireIn < DateTime.UtcNow)
            return Result<TokensResponse>.Failure("Refresh token has expired");

        var user = refreshToken.User;

        await _refreshTokensRepository.DeleteAsync(refreshToken.Id);

        refreshToken = new RefreshTokenEntity
        {
            Id = Guid.NewGuid(),
            ExpireIn = DateTime.UtcNow.AddHours(_options.RefreshTokenValidityHours),
            Token = _tokenProvider.GenerateRefreshToken(),
            UserId = user.Id
        };
        await _refreshTokensRepository.AddAsync(refreshToken);

        var accessToken = _tokenProvider.Generate(user);

        var response = new TokensResponse()
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken.Token
        };
        return Result<TokensResponse>.Success(response);
    }
}