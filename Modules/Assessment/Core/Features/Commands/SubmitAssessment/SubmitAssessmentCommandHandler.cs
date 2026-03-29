using System;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using CareerPath.Assessment.Core.Contracts;
using CareerPath.Assessment.Core.Entities;
using CareerPath.Assessment.Core.DTOs;
using CareerPath.Shared.Responses;
using MediatR;

namespace CareerPath.Assessment.Core.Features.Commands.SubmitAssessment;

public class SubmitAssessmentCommandHandler : IRequestHandler<SubmitAssessmentCommand, Result<Guid>>
{
    private readonly IAiModelClient _aiClient;
    private readonly IAssessmentRepository _repository;
    private readonly ICareerMappingProvider _mappingProvider;

    public SubmitAssessmentCommandHandler(
        IAiModelClient aiClient,
        IAssessmentRepository repository,
        ICareerMappingProvider mappingProvider)
    {
        _aiClient = aiClient;
        _repository = repository;
        _mappingProvider = mappingProvider;
    }

    public async Task<Result<Guid>> Handle(SubmitAssessmentCommand request, CancellationToken cancellationToken)
    {
        // 1. Call the AI Model
        var payload = new AssessmentSubmissionPayload(request.Answers);
        var aiResult = await _aiClient.GetPredictionsAsync(payload, cancellationToken);

        if (!aiResult.IsSuccess || aiResult.Value == null)
            return Result<Guid>.Failure(aiResult.Error);

        var topMatches = aiResult.Value.TopMatches.OrderBy(m => m.Rank).ToList();
        if (topMatches.Count < 3)
            return Result<Guid>.Failure("The AI model did not return the required top 3 predictions.");

        // 2. Construct Domain Entities
        var submissionId = Guid.NewGuid();

        var submission = new AssessmentSubmission
        {
            Id = submissionId,
            UserId = request.UserId,
            RawAnswersJson = JsonSerializer.Serialize(request.Answers),
            SubmittedAt = DateTime.UtcNow
        };

        var assessmentResult = new AssessmentResult
        {
            PrimaryJobLabel = topMatches[0].JobLabel,
            SecondaryJobLabel = topMatches[1].JobLabel,
            TertiaryJobLabel = topMatches[2].JobLabel,

            Id = Guid.NewGuid(),
            AssessmentSubmissionId = submissionId,
            PrimaryConfidence = (decimal)topMatches[0].Confidence,
            SecondaryConfidence = (decimal)topMatches[1].Confidence,
            TertiaryConfidence = (decimal)topMatches[2].Confidence,
            GeneratedAt = DateTime.UtcNow
        };

        // Link the relationship
        submission.Result = assessmentResult;

        // 4. Persist and Return
        return await _repository.AddSubmissionAsync(submission, cancellationToken);
    }
}