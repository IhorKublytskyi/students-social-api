namespace backend.Application.Interfaces;

public interface ITokenReader
{
    string ReadToken(string accessToken, string type);
}