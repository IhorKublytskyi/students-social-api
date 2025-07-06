using FluentValidation;
using StudentsSocial.Application.Models.RequestModels;

namespace StudentsSocial.Application.Validators;

public class RegistrationRequestValidator : AbstractValidator<RegistrationRequest>
{
    public RegistrationRequestValidator()
    {
        RuleFor(r => r.FirstName)
            .NotNull()
            .MaximumLength(20)
            .MinimumLength(2);
        RuleFor(r => r.LastName)
            .NotNull()
            .MaximumLength(20)
            .MinimumLength(2);
        RuleFor(r => r.Username)
            .NotNull()
            .MaximumLength(20)
            .MinimumLength(2);
        RuleFor(r => r.BirthDate)
            .NotNull()
            .NotEmpty()
            .Matches(@"(\b\d{2}\.\d{2}.\d{4}\b)|(\b\d{4}-\d{2}-\d{2}\b)|(\b\d{2}\/\d{2}\/\d{4}\b)")
            .Must(date =>
            {
                var result = DateOnly.TryParse(date, out DateOnly parsedDate);
                return result;
            })
            .WithMessage("Invalid date");
        RuleFor(r => r.Email)
            .NotNull()
            .Matches(@"^(?!\.)([\w-]|((?<!\.)\.)){2,64}(?<!\.)@(?=.*?\.)(?!\.|-)([\w-]|((?<!\.)\.)){2,64}(?<!\.|-)$");
        RuleFor(r => r.Password)
            .NotNull()
            .Matches(@"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[_#?!@$%^&*-]).{8,255}$");
        
    }
}