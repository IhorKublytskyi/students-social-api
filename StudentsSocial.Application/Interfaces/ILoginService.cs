using StudentsSocial.Application.Models.ResponseModels;
using StudentsSocial.Core.Results;

namespace StudentsSocial.Application.Interfaces;

public interface ILoginService
{
    Task<Result<TokensResponse>> LoginAsync(string email, string password);
}