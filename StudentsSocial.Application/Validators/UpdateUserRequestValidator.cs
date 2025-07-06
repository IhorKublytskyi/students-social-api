using FluentValidation;
using StudentsSocial.Application.Models.RequestModels;

namespace StudentsSocial.Application.Validators;

public class UpdateUserRequestValidator : AbstractValidator<UpdateUserRequest>
{
    public UpdateUserRequestValidator()
    {
        RuleFor(r => r.Email)
            .NotNull()
            .NotEmpty()
            .Matches(@"^(?!\.)([\w-]|((?<!\.)\.)){2,64}(?<!\.)@(?=.*?\.)(?!\.|-)([\w-]|((?<!\.)\.)){2,64}(?<!\.|-)$");
        RuleFor(r => r.Username)
            .NotNull()
            .MaximumLength(20)
            .MinimumLength(2);
        RuleFor(r => r.FirstName)
            .NotNull()
            .MaximumLength(20)
            .MinimumLength(2);
        RuleFor(r => r.LastName)
            .NotNull()
            .MaximumLength(20)
            .MinimumLength(2);
        RuleFor(r => r.ProfilePicture)
            .NotNull()
            .NotEmpty()
            .Must(p => p.Length < 5 * 1024 * 1024);
        RuleFor(r => r.Status)
            .NotNull()
            .MaximumLength(50);
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
        RuleFor(r => r.Biography)
            .NotNull()
            .NotEmpty()
            .MaximumLength(500);
    }
}