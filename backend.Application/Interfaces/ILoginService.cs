using backend.Application.Models.ResponseModels;
using backend.Core.Results;

namespace backend.Application.Interfaces;

public interface ILoginService
{
    Task<Result<TokensResponse>> LoginAsync(string email, string password);
}