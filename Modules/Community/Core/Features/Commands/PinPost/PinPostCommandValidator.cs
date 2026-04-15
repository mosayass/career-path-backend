using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace CareerPath.Community.Core.Features.Commands.PinPost
{
    public class PinPostCommandValidator : AbstractValidator<PinPostCommand>
    {
        public PinPostCommandValidator()
        {
            RuleFor(x => x.InstructorId).NotEmpty();
            RuleFor(x => x.PostId).NotEmpty();
            RuleFor(x => x.CommunityId).NotEmpty();
        }
    }
}
