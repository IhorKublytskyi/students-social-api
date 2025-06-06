using backend.Core.Entities;

namespace backend.Core.Interfaces;

public interface ITokenProvider
{
    string Generate(UserEntity user);
    string GenerateRefreshToken();
}