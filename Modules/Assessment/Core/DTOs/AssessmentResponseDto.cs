using System;
using CareerPath.Shared.Contracts.Careers;

namespace CareerPath.Assessment.Core.DTOs;

public record AssessmentResponseDto(
    Guid AssessmentId,
    DateTime CompletedAt,
    CareerMatchDetailsDto MatchDetails
);