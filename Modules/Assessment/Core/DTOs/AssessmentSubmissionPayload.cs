using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CareerPath.Assessment.Core.DTOs;

public record AssessmentSubmissionPayload(
    [property: JsonPropertyName("features")] IEnumerable<float> Features
);