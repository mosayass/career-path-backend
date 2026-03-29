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
        public decimal PrimaryConfidence { get; set; }

        // Top 2 Prediction
        public decimal SecondaryConfidence { get; set; }

        // Top 3 Prediction
        public decimal TertiaryConfidence { get; set; }

        public DateTime GeneratedAt { get; set; } = DateTime.UtcNow;

        //  Storing the raw AI integer labels ---
        public int PrimaryJobLabel { get; set; }
        public int SecondaryJobLabel { get; set; }
        public int TertiaryJobLabel { get; set; }

    }
}
