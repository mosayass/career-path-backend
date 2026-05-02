using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace CareerPath.Community.Core.Features.Commands.CreatePost
{
    public class CreatePostCommandValidator : AbstractValidator<CreatePostCommand>
    {
        public CreatePostCommandValidator()
        {
            RuleFor(x => x.UserId).NotEmpty();
            RuleFor(x => x.CommunityId).NotEmpty();

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(250).WithMessage("Title cannot exceed 250 characters.");

            RuleFor(x => x.CareerTag)
                .NotEmpty().WithMessage("A Career Tag is required.")
                .MaximumLength(100);

            // Optional Body limit
            RuleFor(x => x.Body)
                .MaximumLength(5000).When(x => !string.IsNullOrEmpty(x.Body));
        }
    }
}
