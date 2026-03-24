using System.Collections.Generic;

namespace CareerPath.Assessment.Core.DTOs;

public record CareerPrediction(
    int Rank,
    int JobLabel,
    float Confidence
);

public record AiPredictionResponse(
    IEnumerable<CareerPrediction> TopMatches
);