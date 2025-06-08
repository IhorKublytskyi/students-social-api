using backend.Application.RequestModels;
using backend.Core.Results;

namespace backend.Application.Interfaces;

public interface IRegistrationService
{
    Task<Result> RegisterAsync(RegistrationRequest request);
}