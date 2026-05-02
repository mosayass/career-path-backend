using System;

namespace CareerPath.Shared.Contracts.Profiles;

public record ProfileDataResponse(
    Guid UserId,
    string DisplayName,
    string? AvatarUrl);