using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using backend.Application;
using backend.Application.Interfaces;
using backend.Core.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace backend.Infrastructure;

public class TokenProvider : ITokenProvider
{
    private readonly JwtOptions _options;

    public TokenProvider(IOptions<JwtOptions> options)
    {
        _options = options.Value;
    }

    public string Generate(UserEntity user)
    {
        var claims = new List<Claim>
        {
            new("Id", user.Id.ToString()),
            new("Email", user.Email),
            new("Username", user.Username),
            new("FirstName", user.FirstName),
            new("LastName", user.LastName)
        };
        var signingCredentials = new SigningCredentials
        (
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey)),
            SecurityAlgorithms.HmacSha256
        );
        var token = new JwtSecurityToken
        (
            claims: claims,
            signingCredentials: signingCredentials,
            expires: DateTime.UtcNow.AddMinutes(_options.AccessTokenValidityMins)
        );

        var accessToken = new JwtSecurityTokenHandler().WriteToken(token);

        return accessToken;
    }

    public string GenerateRefreshToken()
    {
        return Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
    }
}