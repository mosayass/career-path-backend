using FluentValidation;
using CareerPath.Shared.Contracts.Careers;

namespace CareerPath.Careers.Core.Features.Queries.GetCareerMatchDetails;

public class GetCareerMatchDetailsQueryValidator : AbstractValidator<GetCareerMatchDetailsQuery>
{
    public GetCareerMatchDetailsQueryValidator()
    {
        RuleFor(x => x.AiLabelId)
            .GreaterThanOrEqualTo(0)
            .WithMessage("AI Label ID must be a valid positive integer.");
    }
}