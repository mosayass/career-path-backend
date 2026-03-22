using System;
using System.Collections.Generic;

namespace CareerPath.Assessment.Core.DTOs;

public record AssessmentSubmissionPayload(
    Guid UserId,
    IEnumerable<string> Answers 
);