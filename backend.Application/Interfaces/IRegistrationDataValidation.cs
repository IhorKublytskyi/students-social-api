using backend.Application.RequestModels;
using backend.Core.Results;

namespace backend.Application.Interfaces;

public interface IRegistrationDataValidation
{
    Result Validate(RegistrationRequest request);
}