using backend.Core.Entities;

namespace backend.Application.Interfaces;

public interface ITokenProvider
{
    string Generate(UserEntity user);
    string GenerateRefreshToken();
}