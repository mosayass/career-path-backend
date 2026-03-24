using System;
using System.Collections.Generic;
using System.Text;

namespace CareerPath.Assessment.Core.Entities
{
    public class AssessmentResult
    {
        public Guid Id { get; set; }

        // Foreign Key
        public Guid AssessmentSubmissionId { get; set; }
        public AssessmentSubmission Submission { get; set; } = null!;

        // Top 1 Prediction
        public string PrimaryCareer { get; set; } = string.Empty;
        public decimal PrimaryConfidence { get; set; }

        // Top 2 Prediction
        public string SecondaryCareer { get; set; } = string.Empty;
        public decimal SecondaryConfidence { get; set; }

        // Top 3 Prediction
        public string TertiaryCareer { get; set; } = string.Empty;
        public decimal TertiaryConfidence { get; set; }

        public DateTime GeneratedAt { get; set; } = DateTime.UtcNow;
    }
}
