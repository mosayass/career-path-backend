using FluentValidation;

namespace CareerPath.Identity.Core.Features.Queries.Login;

public class LoginQueryValidator : AbstractValidator<LoginQuery>
{
    public LoginQueryValidator()
    {
        RuleFor(x => x.RequestDto.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("A valid email format is required.");

        RuleFor(x => x.RequestDto.Password)
            .NotEmpty().WithMessage("Password is required.");
    }
}