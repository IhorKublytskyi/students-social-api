using backend.Application.Interfaces;
using backend.Application.RequestModels;
using backend.Core.Results;

namespace backend.Application;

public class RegistrationDataValidation : IRegistrationDataValidation
{
    public Result Validate(RegistrationRequest request)
    {
        if (request == null)
            return Result.Failure("Request can not be null");

        if (string.IsNullOrWhiteSpace(request.FirstName))
            return Result.Failure("First name is required");
        if (string.IsNullOrWhiteSpace(request.LastName))
            return Result.Failure("Last name is required");
        if (string.IsNullOrWhiteSpace(request.Username))
            return Result.Failure("Username is required");
        if (string.IsNullOrWhiteSpace(request.Email))
            return Result.Failure("Email is required");
        if (string.IsNullOrWhiteSpace(request.BirthDate))
            return Result.Failure("Birth date is required");
        if (string.IsNullOrWhiteSpace(request.Password))
            return Result.Failure("Password is required");
        
        return Result.Success();
    }
}