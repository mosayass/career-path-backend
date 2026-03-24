using System;
using System.Collections.Generic;
using System.Text;

namespace CareerPath.Assessment.Core.Entities
{
    public class AssessmentSubmission
    {
        public Guid Id { get; set; }

        // Links back to the Identity module's user
        public Guid UserId { get; set; }

        // Storing the answers as a JSON string for future AI model retraining/auditing
        public string RawAnswersJson { get; set; } = string.Empty;

        public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;

        // Navigation property
        public AssessmentResult? Result { get; set; }
    }
}
