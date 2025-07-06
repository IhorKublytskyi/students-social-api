using StudentsSocial.Core.Entities;

namespace StudentsSocial.Application.Interfaces;

public interface ITokenProvider
{
    string Generate(UserEntity user);
    string GenerateRefreshToken();
}