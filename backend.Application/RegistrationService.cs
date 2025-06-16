using backend.Application.Interfaces;
using backend.Application.Models.RequestModels;
using backend.Core.Entities;
using backend.Core.Results;
using backend.Core.Interfaces.Repositories;

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

    public async Task<Result> RegisterAsync(RegistrationRequest request)
    {
        var existedUser = await _repository.GetByUsername(request.Username);
        if (existedUser != null)
            return Result.Failure("Username is already taken");

        existedUser = await _repository.GetByEmail(request.Email);
        if (existedUser != null)
            return Result.Failure("Email is already taken");
        
        var user = new UserEntity
        {
            Id = Guid.NewGuid(),
            Email = request.Email,
            PasswordHash = _hasher.HashPassword(request.Password),
            Username = request.Username,
            FirstName = request.FirstName,
            LastName = request.LastName,
            BirthDate = DateTime.Parse(request.BirthDate),
            CreatedAt = DateTime.UtcNow
        };

        await _repository.Add(user);

        return Result.Success();
    }
}