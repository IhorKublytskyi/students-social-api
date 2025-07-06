using StudentsSocial.Core.Results;

namespace StudentsSocial.Application.Interfaces;

public interface ITokenReader
{
    Result<string> ReadToken(string accessToken, string type);
}