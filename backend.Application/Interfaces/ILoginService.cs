using backend.Core.Results;

namespace backend.Application.Interfaces;

public interface ILoginService
{
    Task<Result<(string, string)>> LoginAsync(string email, string password);
}