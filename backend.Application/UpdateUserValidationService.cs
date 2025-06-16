using backend.Application.Interfaces;
using backend.Application.Models.RequestModels;
using backend.Core.Entities;

namespace backend.Application;

public class UpdateUserValidationService : IUpdateUserValidationService
{
    public UpdateUserRequest Validate(UpdateUserRequest request, UserEntity user)
    {
        if (string.IsNullOrWhiteSpace(request.Email))
            request.Email = user.Email;
        if (string.IsNullOrWhiteSpace(request.Username))
            request.Username = user.Username;
        if (string.IsNullOrWhiteSpace(request.FirstName))
            request.FirstName = user.FirstName;
        if (string.IsNullOrWhiteSpace(request.LastName))
            request.LastName = user.LastName;
        if (string.IsNullOrWhiteSpace(request.Status))
            request.Status = user.Status;
        if (string.IsNullOrWhiteSpace(request.BirthDate))
            request.BirthDate = user.BirthDate.ToString("en-US");
        if (string.IsNullOrWhiteSpace(request.Biography))
            request.Biography = user.Biography;
        if (request.ProfilePicture.Length == 0 || request.ProfilePicture == null)
            request.ProfilePicture = user.ProfilePicture;

        return request;
    }
}