using backend.Core.Entities;

namespace backend.Infrastructure.Interfaces;

public interface ITokenProvider
{
    string Generate(UserEntity user);
    string GenerateRefreshToken();
}