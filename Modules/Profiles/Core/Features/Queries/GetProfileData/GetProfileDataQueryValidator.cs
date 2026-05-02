using CareerPath.Shared.Contracts.Profiles;
using FluentValidation;

namespace CareerPath.Profiles.Core.Features.Queries.GetProfileData;

public class GetProfileDataQueryValidator : AbstractValidator<GetProfileDataQuery>
{
    public GetProfileDataQueryValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("User ID is required.");
    }
}