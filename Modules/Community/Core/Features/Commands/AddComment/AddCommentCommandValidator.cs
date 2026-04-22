using FluentValidation;

namespace CareerPath.Community.Core.Features.Commands.AddComment
{
    public class AddCommentCommandValidator : AbstractValidator<AddCommentCommand>
    {
        public AddCommentCommandValidator()
        {
            RuleFor(x => x.UserId).NotEmpty();
            RuleFor(x => x.PostId).NotEmpty();

            RuleFor(x => x.Body)
                .NotEmpty().WithMessage("Comment body cannot be empty.")
                .MaximumLength(2500).WithMessage("Comment cannot exceed 2500 characters.");
        }
    }
}
