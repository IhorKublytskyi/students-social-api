using StudentsSocial.Application.Interfaces;
using StudentsSocial.Application.Models.ResponseModels;
using StudentsSocial.Core.Entities;
using StudentsSocial.Core.Results;
using StudentsSocial.Core.Interfaces.Repositories;
using Microsoft.Extensions.Options;

namespace StudentsSocial.Application;

public class LoginService : ILoginService
{
    private readonly IPasswordHasher _hasher;
    private readonly JwtOptions _options;
    private readonly IRefreshTokensRepository _refreshTokensRepository;
    private readonly ITokenProvider _tokenProvider;
    private readonly IUsersRepository _usersRepository;

    public LoginService(
        IUsersRepository usersRepository,
        IRefreshTokensRepository refreshTokensRepository,
        ITokenProvider tokenProvider,
        IPasswordHasher hasher,
        IOptions<JwtOptions> options)
    {
        _usersRepository = usersRepository;
        _refreshTokensRepository = refreshTokensRepository;
        _tokenProvider = tokenProvider;
        _hasher = hasher;
        _options = options.Value;
    }

    public async Task<Result<TokensResponse>> LoginAsync(string email, string password)
    {
        var user = await _usersRepository.GetByEmailAsync(email);
        if (user == null)
            return Result<TokensResponse>.Failure("User not found");
        
        if (!_hasher.VerifyPassword(password, user.PasswordHash))
            return Result<TokensResponse>.Failure("Invalid email or password");
        
        var accessToken = _tokenProvider.Generate(user);
        var refreshToken = new RefreshTokenEntity
        {
            Id = Guid.NewGuid(),
            Token = _tokenProvider.GenerateRefreshToken(),
            ExpireIn = DateTime.UtcNow.AddHours(_options.RefreshTokenValidityHours),
            UserId = user.Id
        };
        await _refreshTokensRepository.AddAsync(refreshToken);

        var response = new TokensResponse()
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken.Token
        };
        return Result<TokensResponse>.Success(response);
    }
}