using StudentsSocial.Application.Models.RequestModels;
using StudentsSocial.Application.Models.ResponseModels;
using StudentsSocial.Core.Models.FilterModels;
using StudentsSocial.Core.Results;

namespace StudentsSocial.Application.Interfaces;
public interface IUserService
{
    Task<Result<List<UserResponse>>> Get(UserFilter filter);
    Task<Result<UserResponse?>> GetUser(Guid id);
    Task<Result> Update(Guid id, UpdateUserRequest request);
}