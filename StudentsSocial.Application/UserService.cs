using StudentsSocial.Application.Interfaces;
using StudentsSocial.Application.Models.RequestModels;
using StudentsSocial.Application.Models.ResponseModels;
using StudentsSocial.Application.Validators;
using StudentsSocial.Core.Entities;
using StudentsSocial.Core.Interfaces.Repositories;
using StudentsSocial.Core.Models.FilterModels;
using StudentsSocial.Core.Results;
namespace StudentsSocial.Application;

public class UserService : IUserService
{
    private readonly IUsersRepository _repository;
    private readonly IUserUpdateMerger _merger;

    public UserService(
        IUsersRepository repository, 
        IUserUpdateMerger merger)
    {
        _repository = repository;
        _merger = merger;
    }

    public async Task<Result<List<UserResponse>>> Get(UserFilter filter)
    {
        var filteredUsers = await _repository.GetByFilterAsync(filter);
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
        var user = await _repository.GetByIdAsync(id);
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
        var existedUserById = await _repository.GetByIdAsync(id);
        if(existedUserById == null)
            return Result.Failure("User not found");

        var existedUserByUsername = await _repository.GetByUsernameAsync(request.Username);
        if(existedUserByUsername != null)
            return Result.Failure("Username is already taken");

        var user = _merger.Merge(existedUserById, request);

        await _repository.UpdateAsync(user.Id, user);
        
        return Result.Success();
    }
}