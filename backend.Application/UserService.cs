using backend.Application.Interfaces;
using backend.Application.Models.RequestModels;
using backend.Application.Models.ResponseModels;
using backend.Core.Interfaces.Repositories;
using backend.Core.Models.FilterModels;
using backend.Core.Results;
using Microsoft.AspNetCore.Http;
namespace backend.Application;

public class UserService : IUserService
{
    private readonly IUsersRepository _repository;
    private readonly IUpdateUserValidationService _updateUserValidator;

    public UserService(
        IUsersRepository repository, 
        IUpdateUserValidationService updateUserValidator)
    {
        _repository = repository;
        _updateUserValidator = updateUserValidator;
    }

    public async Task<Result<List<UserResponse>>> Get(UserFilter filter)
    {
        var filteredUsers = await _repository.GetByFilter(filter);
        if(filteredUsers.Count == 0)
            return Result<List<UserResponse>>.Failure("Users not found");
        
        var result = filteredUsers.Select( u =>
        {
            return new UserResponse()
            {
                Id = u.Id,
                Email = u.Email,
                Username = u.Username,
                FirstName = u.FirstName,
                LastName = u.LastName,
                ProfilePicture = u.ProfilePicture,
                Status = u.Status,
                BirthDate = u.BirthDate,
                Biography = u.Biography,
                CreatedAt = u.CreatedAt,
                FollowersCount = u.Followers.Count,
                FollowedCount = u.FollowedUsers.Count,
                PostsCounts = u.Posts.Count
            };
        });
        
        return Result<List<UserResponse>>.Success(result.ToList());
    }
    public async Task<Result<UserResponse>> GetUser(Guid id)
    {
        var user = await _repository.GetById(id);
        if(user == null)
            return Result<UserResponse>.Failure("User not found");
        
        var response = new UserResponse()
        {
            Id = user.Id,
            Email = user.Email,
            Username = user.Username,
            FirstName = user.FirstName,
            LastName = user.LastName,
            ProfilePicture = user.ProfilePicture,
            Status = user.Status,
            BirthDate = user.BirthDate,
            Biography = user.Biography,
            CreatedAt = user.CreatedAt,
            FollowersCount = user.Followers.Count,
            FollowedCount = user.FollowedUsers.Count,
            PostsCounts = user.Posts.Count
        };

        return Result<UserResponse>.Success(response);
    }

    public async Task<Result> Update(Guid id, UpdateUserRequest request)
    {
        var user = await _repository.GetById(id);
        if(user == null)
            return Result.Failure("User not found");

        request = _updateUserValidator.Validate(request, user); 
        
        await _repository.Update(user.Id, request.Email, request.Username, request.FirstName, request.LastName, request.ProfilePicture, request.Status, DateTime.Parse(request.BirthDate), request.Biography);
        
        return Result.Success();
    }
}