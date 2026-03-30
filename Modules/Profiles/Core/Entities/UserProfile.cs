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
        public int? TargetCareerId { get; set; }
        public Guid? LatestAssessmentId { get; set; }
        public bool IsAcceptingDirectMessages { get; set; }

        // Standard Audit Properties
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    public enum ProfileType
    {
        Student = 1,
        HumanMentor = 2,
        AiMentor = 3
    }
}