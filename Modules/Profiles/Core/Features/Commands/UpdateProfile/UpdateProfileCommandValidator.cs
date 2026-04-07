using System;
using FluentValidation;

namespace CareerPath.Profiles.Core.Features.Commands.UpdateProfile
{
    public class UpdateProfileCommandValidator : AbstractValidator<UpdateProfileCommand>
    {
        public UpdateProfileCommandValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("UserId is required.");

            RuleFor(x => x.DisplayName)
                .NotEmpty().WithMessage("DisplayName is required.")
                .MinimumLength(2).WithMessage("DisplayName must be at least 2 characters long.")
                .MaximumLength(100).WithMessage("DisplayName must not exceed 100 characters.");

            RuleFor(x => x.Bio)
                .MaximumLength(500).WithMessage("Bio must not exceed 500 characters.")
                .When(x => !string.IsNullOrWhiteSpace(x.Bio));

            RuleFor(x => x.AvatarUrl)
                .MaximumLength(500).WithMessage("AvatarUrl must not exceed 500 characters.")
                .Must(BeAValidUrl).WithMessage("AvatarUrl must be a valid HTTP or HTTPS URL.")
                .When(x => !string.IsNullOrWhiteSpace(x.AvatarUrl));
        }

        private bool BeAValidUrl(string? url)
        {
            if (string.IsNullOrWhiteSpace(url))
                return true;

            return Uri.TryCreate(url, UriKind.Absolute, out var outUri)
                   && (outUri.Scheme == Uri.UriSchemeHttp || outUri.Scheme == Uri.UriSchemeHttps);
        }
    }
}