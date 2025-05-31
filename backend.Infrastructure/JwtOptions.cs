namespace backend.Infrastructure;

public class JwtOptions
{
    public string SecretKey { get; set; } = string.Empty;
    public int AccessTokenValidityMins { get; set; }
    public int RefreshTokenValidityMins { get; set; }
}