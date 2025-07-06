using StudentsSocial.Application.Interfaces;
using StudentsSocial.Application.Models.RequestModels;
using StudentsSocial.Core.Entities;
using StudentsSocial.Core.Results;
using StudentsSocial.Core.Interfaces.Repositories;

namespace StudentsSocial.Application;

public class RegistrationService : IRegistrationService
{
    private readonly IPasswordHasher _hasher;
    private readonly IUsersRepository _repository;

    public RegistrationService(IPasswordHasher hasher, IUsersRepository repository)
    {
        _hasher = hasher;
        _repository = repository;
    }

    public async Task<Result> RegisterAsync(RegistrationRequest request)
    {
        var existedUserByUsername = await _repository.GetByUsernameAsync(request.Username!);
        if (existedUserByUsername != null)
            return Result.Failure("Username is already taken");

        var existedUserByEmail = await _repository.GetByEmailAsync(request.Email!);
        if (existedUserByEmail != null)
            return Result.Failure("Email is already taken");
        
        var user = new UserEntity
        {
            Id = Guid.NewGuid(),
            Email = request.Email!,
            PasswordHash = _hasher.HashPassword(request.Password!),
            Username = request.Username!,
            FirstName = request.FirstName!,
            LastName = request.LastName!,
            BirthDate = DateTime.Parse(request.BirthDate!),
            CreatedAt = DateTime.UtcNow
        };

        await _repository.AddAsync(user);

        return Result.Success();
    }
}