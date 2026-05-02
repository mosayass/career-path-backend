using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace CareerPath.Community.Core.Features.Commands.CastCommentVote
{
    public class CastCommentVoteCommandValidator : AbstractValidator<CastCommentVoteCommand>
    {
        public CastCommentVoteCommandValidator()
        {
            RuleFor(x => x.UserId).NotEmpty();
            RuleFor(x => x.TargetId).NotEmpty();
        }
    }
}