using backend.Core.Entities;
using backend.Core.Interfaces;
using backend.Core.Interfaces.Repositories;
using backend.Core.Results;

namespace backend.Application;

public class RegistrationService : IRegistrationService
{
    private readonly IPasswordHasher _hasher;
    private readonly IUsersRepository _repository;

    public RegistrationService(IPasswordHasher hasher, IUsersRepository repository)
    {
        _hasher = hasher;
        _repository = repository;
    }

    public async Task<Result> Register(string firstName, string lastName, string username, string email, DateTime birthDate, string password)
    {
        var existedUser = await _repository.GetByUsername(username);
        if(existedUser != null)
            return Result.Failure("Username is already taken");

        existedUser = await _repository.GetByEmail(email);
        if(existedUser != null)
            return Result.Failure("Email is already taken");

        var user = new UserEntity()
        {
            Id = Guid.NewGuid(),
            Email = email,
            PasswordHash = _hasher.HashPassword(password),
            Username = username,
            FirstName = firstName,
            LastName = lastName,
            BirthDate = birthDate,
            CreatedAt = DateTime.UtcNow
        };

        await _repository.Add(user);
        
        return Result.Success();
    }
}