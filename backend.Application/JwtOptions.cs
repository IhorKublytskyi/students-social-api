namespace backend.Application;

public class JwtOptions
{
    public string SecretKey { get; set; } = string.Empty;
    public int AccessTokenValidityMins { get; set; }
    public int RefreshTokenValidityHours { get; set; }
}