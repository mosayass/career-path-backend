using CareerPath.Shared.Contracts.Assessment;
using FluentValidation;

namespace CareerPath.Assessment.Core.Features.Queries.GetUserAiLabels;

public class GetUserAiLabelsQueryValidator : AbstractValidator<GetUserAiLabelsQuery>
{
    public GetUserAiLabelsQueryValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("UserId is required.");
    }
}