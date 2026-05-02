using FluentValidation;

namespace CareerPath.Community.Core.Features.Commands.PinPost
{
    public class PinPostCommandValidator : AbstractValidator<PinPostCommand>
    {
        public PinPostCommandValidator()
        {
            RuleFor(x => x.UserId).NotEmpty();
            RuleFor(x => x.PostId).NotEmpty();
            RuleFor(x => x.CommunityId).NotEmpty();
        }
    }
}
