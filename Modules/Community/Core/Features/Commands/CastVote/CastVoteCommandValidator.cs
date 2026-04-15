using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace CareerPath.Community.Core.Features.Commands.CastVote
{
    public class CastVoteCommandValidator : AbstractValidator<CastVoteCommand>
    {
        public CastVoteCommandValidator()
        {
            RuleFor(x => x.UserId).NotEmpty();
            RuleFor(x => x.TargetId).NotEmpty();
            RuleFor(x => x.TargetType).IsInEnum().WithMessage("Invalid Target Type.");
        }
    }
}
