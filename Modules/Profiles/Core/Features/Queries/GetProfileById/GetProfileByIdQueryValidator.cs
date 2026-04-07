using FluentValidation;

namespace CareerPath.Profiles.Core.Features.Queries.GetProfileById
{
    public class GetProfileByIdQueryValidator : AbstractValidator<GetProfileByIdQuery>
    {
        public GetProfileByIdQueryValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("UserId must be provided and cannot be an empty GUID.");
        }
    }
}