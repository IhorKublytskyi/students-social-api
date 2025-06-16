using backend.Application.Models.RequestModels;
using backend.Core.Results;

namespace backend.Application.Interfaces;

public interface IRegistrationDataValidation
{
    Result Validate(RegistrationRequest request);
}