using StudentsSocial.Application.Interfaces;
using StudentsSocial.Application.Models.RequestModels;
using StudentsSocial.Core.Entities;

namespace StudentsSocial.Application;

public class UserUpdateMerger : IUserUpdateMerger
{
    public UserEntity Merge(UserEntity user, UpdateUserRequest input)
    {
        if(!string.IsNullOrWhiteSpace(input.Email))
            user.Email = input.Email;
        if(!string.IsNullOrWhiteSpace(input.Username))
            user.Username = input.Username;
        if(!string.IsNullOrWhiteSpace(input.FirstName))
            user.FirstName = input.FirstName;
        if(!string.IsNullOrWhiteSpace(input.LastName))
            user.LastName = input.LastName;
        if(input.ProfilePicture != null && input.ProfilePicture.Length != 0)
            user.ProfilePicture = input.ProfilePicture;
        if(!string.IsNullOrWhiteSpace(input.Status))
            user.Status = input.Status;
        if(!string.IsNullOrWhiteSpace(input.BirthDate))
            user.BirthDate = DateTime.Parse(input.BirthDate);
        if(!string.IsNullOrWhiteSpace(input.Biography))
            user.Biography = input.Biography;

        return user;
    }
}