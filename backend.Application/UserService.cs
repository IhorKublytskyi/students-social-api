using backend.Application.Interfaces;
using backend.Application.ResponseModels;
using backend.Core.Entities;
using backend.Core.Interfaces.Repositories;
using backend.Core.Models.FilterModels;
using backend.Core.Results;

namespace backend.Application;

public class UserService : IUserService
{
    private readonly IUsersRepository _repository;
    private readonly ISubscriptionsRepository _subscriptionsRepository;
    private readonly IPostsRepository _postsRepository;

    public UserService(IUsersRepository repository, ISubscriptionsRepository subscriptionsRepository)
    {
        _repository = repository;
        _subscriptionsRepository = subscriptionsRepository;
    }

    public async Task<Result<UserInfoResponse>> GetUserInfo(Guid id)
    {
        var user = await _repository.GetById(id);

        if (user == null)
            return Result<UserInfoResponse>.Failure("User not found");

        var subscriptions =  GetUserSubscriptions(user);

        var userInfo = new UserInfoResponse()
        {
            Email = user.Email,
            Username = user.Username,
            FirstName = user.FirstName,
            LastName = user.LastName,
            ProfilePicture = user.ProfilePicture,
            Status = user.Status,
            BirthDate = user.BirthDate,
            Biography = user.Biography,
            CreatedAt = user.CreatedAt,
            FollowersCount = subscriptions.followersCount,
            FollowedCount = subscriptions.followedCount
        };
        
        return Result<UserInfoResponse>.Success(userInfo);
    }

    public async Task<Result<List<UserInfoResponse>>> Get(UserFilter filter)
    {
        var filteredUsers = await _repository.GetByFilter(filter);
        if(filteredUsers.Count == 0)
            return Result<List<UserInfoResponse>>.Failure("Users not found");
        
        var result = filteredUsers.Select(u =>
        {
            var subscriptions =  GetUserSubscriptions(u);
            
            return new UserInfoResponse()
            {
                Email = u.Email,
                Username = u.Username,
                FirstName = u.FirstName,
                LastName = u.LastName,
                ProfilePicture = u.ProfilePicture,
                Status = u.Status,
                BirthDate = u.BirthDate,
                Biography = u.Biography,
                CreatedAt = u.CreatedAt,
                FollowersCount = subscriptions.followersCount,
                FollowedCount = subscriptions.followedCount
            };
        }).ToList();
        
        return Result<List<UserInfoResponse>>.Success(result);
    }
    public async Task<Result<UserInfoResponse?>> GetUser(string username)
    { 
        var user = await _repository.GetByUsername(username);
        if(user == null)
            return Result<UserInfoResponse?>.Failure("User not found");

        var subscriptions = GetUserSubscriptions(user);
        var postsCount = await GetPosts(user);
        
        var userInfo = new UserInfoResponse()
        {
            Email = user.Email,
            Username = user.Username,
            FirstName = user.FirstName,
            LastName = user.LastName,
            ProfilePicture = user.ProfilePicture,
            Status = user.Status,
            BirthDate = user.BirthDate,
            Biography = user.Biography,
            CreatedAt = user.CreatedAt,
            FollowersCount = subscriptions.followersCount,
            FollowedCount = subscriptions.followedCount,
            PostsCounts = postsCount
        };

        return Result<UserInfoResponse>.Success(userInfo);
    }

    private async Task<int> GetPosts(UserEntity user)
    {
        var posts = await _postsRepository.GetByUserId(user.Id);

        return posts.Count;
    }
    private (int followersCount, int followedCount) GetUserSubscriptions(UserEntity user)
    {
        int followersCount = _subscriptionsRepository.GetFollowers(user);
        int followedCount = _subscriptionsRepository.GetFollowed(user);

        return (followersCount, followedCount);
    }
}