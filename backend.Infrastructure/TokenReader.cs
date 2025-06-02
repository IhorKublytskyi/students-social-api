using System.IdentityModel.Tokens.Jwt;
using backend.Infrastructure.Interfaces;

namespace backend.Infrastructure;

public class TokenReader : ITokenReader
{
    public string? ReadToken(string accessToken, string type)
    {
        if (string.IsNullOrWhiteSpace(type) || string.IsNullOrWhiteSpace(type))
            return null;
        
        var token = new JwtSecurityTokenHandler().ReadJwtToken(accessToken);

        var value = token.Claims.FirstOrDefault(c => c.Type == type).Value;

        return value;
    }
}