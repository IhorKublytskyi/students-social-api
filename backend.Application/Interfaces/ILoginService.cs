using backend.Application.ResponseModels;
using backend.Core.Results;

namespace backend.Application.Interfaces;

public interface ILoginService
{
    Task<Result<TokensResponse>> LoginAsync(string email, string password);
}