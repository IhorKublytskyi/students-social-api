namespace backend.Core.Interfaces;

public interface ITokenReader
{
    string ReadToken(string accessToken, string type);
}