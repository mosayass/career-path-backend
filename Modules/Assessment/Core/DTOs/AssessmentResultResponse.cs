using System;

namespace CareerPath.Assessment.Core.DTOs;

public record AssessmentResultResponse(
    Guid AssessmentId,
    DateTime SubmittedAt,
    string PrimaryCareer,
    decimal PrimaryConfidence,
    string SecondaryCareer,
    decimal SecondaryConfidence,
    string TertiaryCareer,
    decimal TertiaryConfidence
);