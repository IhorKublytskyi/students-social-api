using FluentValidation;
using StudentsSocial.Application.Models.RequestModels;

namespace StudentsSocial.Application.Validators;

public class LoginRequestValidator : AbstractValidator<LoginRequest>
{
    public LoginRequestValidator()
    {
        //Email
        RuleFor(r => r.Email)
            .NotNull()
            .NotEmpty()
            .EmailAddress()
            .MaximumLength(254)
            .MinimumLength(14);
        
        //Password
        RuleFor(r => r.Password)
            .NotNull()
            .NotEmpty();
    }
}