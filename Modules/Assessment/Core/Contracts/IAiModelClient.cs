using System.Threading;
using System.Threading.Tasks;
using CareerPath.Assessment.Core.DTOs;
using CareerPath.Shared.Responses;

namespace CareerPath.Assessment.Core.Contracts;

public interface IAiModelClient
{
    Task<Result<AiPredictionResponse>> GetPredictionsAsync(
        AssessmentSubmissionPayload payload,
        CancellationToken cancellationToken = default);
}