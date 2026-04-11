using System;
using MediatR;
// Assuming this namespace based on your architectural rules
using CareerPath.Shared.Responses;

namespace CareerPath.Profiles.Core.Features.Commands.UpdateProfile
{
    public record UpdateProfileCommand(
        Guid UserId,
        string DisplayName,
        string? Bio,
        string? AvatarUrl
    ) : IRequest<Result<Unit>>;
}