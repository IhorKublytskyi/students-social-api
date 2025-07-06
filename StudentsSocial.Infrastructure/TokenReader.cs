using System.IdentityModel.Tokens.Jwt;
using StudentsSocial.Application.Interfaces;
using StudentsSocial.Core.Results;

namespace StudentsSocial.Infrastructure;

public class TokenReader : ITokenReader
{
    public Result<string> ReadToken(string accessToken, string type)
    {
        if(string.IsNullOrWhiteSpace(type))
            return Result<string>.Failure("Type can`t be null");

        var token = new JwtSecurityTokenHandler().ReadJwtToken(accessToken);

        var value = token.Claims.FirstOrDefault(c => c.Type == type).Value;

        return value == null ? Result<string>.Failure("Claim was not found") : Result<string>.Success(value);
    }
}