using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace CareerPath.Community.Core.Features.Commands.CastPostVote
{
    public class CastPostVoteCommandValidator : AbstractValidator<CastPostVoteCommand>
    {
        public CastPostVoteCommandValidator()
        {
            RuleFor(x => x.UserId).NotEmpty();
            RuleFor(x => x.PostId).NotEmpty();
        }
    }
}
