using backend.Application.Interfaces;
using backend.Core.Entities;
using backend.Core.Results;
using backend.Infrastructure;
using backend.Core.Interfaces.Repositories;
using Microsoft.Extensions.Options;

namespace backend.Application;

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

    public async Task<Result<(string, string)>> LoginAsync(string email, string password)
    {
        var user = await _usersRepository.GetByEmail(email);
        if (user == null)
            return Result<(string, string)>.Failure("User not found");
        
        if (!_hasher.VerifyPassword(password, user.PasswordHash))
            return Result<(string, string)>.Failure("Invalid email or password");
        
        var accessToken = _tokenProvider.Generate(user);
        var refreshToken = new RefreshTokenEntity
        {
            Id = Guid.NewGuid(),
            Token = _tokenProvider.GenerateRefreshToken(),
            ExpireIn = DateTime.UtcNow.AddHours(_options.RefreshTokenValidityMins),
            UserId = user.Id
        };
        await _refreshTokensRepository.Add(refreshToken);

        return Result<(string, string)>.Success((accessToken, refreshToken.Token));
    }
}