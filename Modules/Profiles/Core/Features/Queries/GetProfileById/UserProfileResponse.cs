using System;

namespace CareerPath.Profiles.Core.Features.Queries.GetProfileById
{
    // A clean record containing only the safe, public-facing data
    public record UserProfileResponse(
        Guid UserId,
        string DisplayName,
        string? Bio,
        string? AvatarUrl,
        string Type, // Serialized as string for cleaner API consumption
        int PrimarySectorId,
        Guid? TargetCareerId,
        Guid? LatestAssessmentId,
        int ReputationScore,
        bool IsAcceptingDirectMessages
    );
}