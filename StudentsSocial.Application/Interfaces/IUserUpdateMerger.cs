using StudentsSocial.Application.Models.RequestModels;
using StudentsSocial.Core.Entities;

namespace StudentsSocial.Application.Interfaces;

public interface IUserUpdateMerger
{
    UserEntity Merge(UserEntity user, UpdateUserRequest input);
}