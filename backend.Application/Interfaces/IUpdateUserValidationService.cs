using backend.Application.Models.RequestModels;
using backend.Core.Entities;

namespace backend.Application.Interfaces;

public interface IUpdateUserValidationService
{
    UpdateUserRequest Validate(UpdateUserRequest request, UserEntity user);
}