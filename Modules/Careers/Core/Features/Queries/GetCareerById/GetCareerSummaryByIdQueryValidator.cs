using FluentValidation;
using System;

namespace CareerPath.Careers.Core.Features.Queries.GetCareerById;

public class GetCareerSummaryByIdQueryValidator : AbstractValidator<GetCareerSummaryByIdQuery>
{
    public GetCareerSummaryByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Career ID is required.");
    }
}