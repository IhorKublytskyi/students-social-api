namespace backend.Infrastructure.Interfaces;

public interface ITokenReader
{
    string ReadToken(string accessToken, string type);
}