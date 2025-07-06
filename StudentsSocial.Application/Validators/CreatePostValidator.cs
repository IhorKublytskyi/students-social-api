using FluentValidation;
using StudentsSocial.Application.Models.RequestModels;

namespace StudentsSocial.Application.Validators;

public class CreatePostValidator : AbstractValidator<CreatePostRequest>
{
    public CreatePostValidator()
    {
        RuleFor(r => r.UserId)
            .NotNull()
            .NotEmpty();
        RuleFor(r => r.Title)
            .NotNull()
            .MaximumLength(255)
            .MinimumLength(1);
        RuleFor(r => r.Description)
            .NotNull()
            .MaximumLength(500);
    }
}