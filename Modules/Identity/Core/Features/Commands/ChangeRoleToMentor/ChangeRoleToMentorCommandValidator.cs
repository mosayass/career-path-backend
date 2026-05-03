namespace CareerPath.Identity.Core.Features.Commands.ChangeRoleToMentor;

using FluentValidation;

public class ChangeRoleToMentorCommandValidator : AbstractValidator<ChangeRoleToMentorCommand>
{
    public ChangeRoleToMentorCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("A valid email address is required.");
    }
}