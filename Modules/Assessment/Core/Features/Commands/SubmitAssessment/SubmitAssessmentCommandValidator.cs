using CareerPath.Assessment.Core.Features.Commands.SubmitAssessment;
using FluentValidation;
using System.Linq;

namespace CareerPath.Assessment.Core.Features.Commands.SubmitAssessment;

public class SubmitAssessmentCommandValidator : AbstractValidator<SubmitAssessmentCommand>
{
    public SubmitAssessmentCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("User ID is required to submit an assessment.");

        RuleFor(x => x.Answers)
            .NotNull()
            .WithMessage("Answers collection cannot be null.")
            .Must(answers => answers != null && answers.Count() == 27)
            .WithMessage("Exactly 27 personality scores must be provided.");
    }
}