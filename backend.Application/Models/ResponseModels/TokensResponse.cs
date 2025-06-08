namespace backend.Application.ResponseModels;

public class TokensResponse
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
}