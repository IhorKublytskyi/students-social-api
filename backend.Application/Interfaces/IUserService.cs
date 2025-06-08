using backend.Application.ResponseModels;
using backend.Core.Entities;
using backend.Core.Models.FilterModels;
using backend.Core.Results;

namespace backend.Application.Interfaces;
public interface IUserService
{
    Task<Result<UserInfoResponse>> GetUserInfo(Guid id);
    Task<Result<List<UserInfoResponse>>> Get(UserFilter filter);
    Task<Result<UserInfoResponse?>> GetUser(string username);
}