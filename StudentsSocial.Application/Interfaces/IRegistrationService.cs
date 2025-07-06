using StudentsSocial.Application.Models.RequestModels;
using StudentsSocial.Core.Results;

namespace StudentsSocial.Application.Interfaces;

public interface IRegistrationService
{
    Task<Result> RegisterAsync(RegistrationRequest request);
}