using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace CareerPath.Community.Core.Features.Commands.EndorseComment
{
    public class EndorseCommentCommandValidator : AbstractValidator<EndorseCommentCommand>
    {
        public EndorseCommentCommandValidator()
        {
            RuleFor(x => x.InstructorId).NotEmpty();
            RuleFor(x => x.CommentId).NotEmpty();
            RuleFor(x => x.CommunityId).NotEmpty();
        }
    }
}
