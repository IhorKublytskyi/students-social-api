using backend.Core.Results;

namespace backend.Core.Interfaces;

public interface IRegistrationService
{
    Task<Result> Register(string firstName, string lastName, string username, string email, DateTime birthDate, string password);
}