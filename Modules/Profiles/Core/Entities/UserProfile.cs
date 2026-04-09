using System;

namespace CareerPath.Profiles.Core.Entities
{
    public class UserProfile
    {
        public Guid UserId { get; set; }
        public string DisplayName { get; set; } = string.Empty;
        public string? Bio { get; set; }
        public string? AvatarUrl { get; set; }

        // Community/Social Hooks
        public ProfileType Type { get; set; }
        public int PrimarySectorId { get; set; }
        public int ReputationScore { get; set; }
        public Guid? TargetCareerId { get; set; }
        public Guid? LatestAssessmentId { get; set; }
        public bool IsAcceptingDirectMessages { get; set; }

        // Standard Audit Properties
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public UserProfile(Guid userId, string displayName, ProfileType type)
        {
            UserId = userId;
            DisplayName = displayName;
            Type = type;
            ReputationScore = 0; // Default
            IsAcceptingDirectMessages = false; // Default
            CreatedAt = DateTime.UtcNow;
        }
    }

    public enum ProfileType
    {
        Student = 1,
        HumanMentor = 2,
        AiMentor = 3
    }
}