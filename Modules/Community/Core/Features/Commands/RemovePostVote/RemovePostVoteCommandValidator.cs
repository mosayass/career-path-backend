using FluentValidation;

namespace CareerPath.Community.Core.Features.Commands.RemovePostVote;

public class RemovePostVoteCommandValidator : AbstractValidator<RemovePostVoteCommand>
{
    public RemovePostVoteCommandValidator()
    {
        RuleFor(x => x.PostId).NotEmpty();
        RuleFor(x => x.UserId).NotEmpty();
    }
}