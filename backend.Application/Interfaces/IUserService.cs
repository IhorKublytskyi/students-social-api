using backend.Application.Models.RequestModels;
using backend.Application.Models.RequestModels;
using backend.Application.Models.ResponseModels;
using backend.Core.Models.FilterModels;
using backend.Core.Results;
using Microsoft.AspNetCore.Http;

namespace backend.Application.Interfaces;
public interface IUserService
{
    Task<Result<List<UserResponse>>> Get(UserFilter filter);
    Task<Result<UserResponse?>> GetUser(Guid id);
    Task<Result> Update(Guid id, UpdateUserRequest request);
}