using CareerPath.Assessment.Core.Contracts;
using CareerPath.Assessment.Core.DTOs;
using CareerPath.Assessment.Core.Entities;
using CareerPath.Shared.Contracts.Careers;
using CareerPath.Shared.IntegrationEvents.Assessment;
using CareerPath.Shared.IntegrationEvents.Contracts;
using CareerPath.Shared.Responses;
using MediatR;
using System.Text.Json;

namespace CareerPath.Assessment.Core.Features.Commands.SubmitAssessment;

public class SubmitAssessmentCommandHandler : IRequestHandler<SubmitAssessmentCommand, Result<Guid>>
{
    private readonly IAiModelClient _aiClient;
    private readonly IAssessmentRepository _repository;
    private readonly IEventCollector _eventCollector;
    private readonly ISender _sender;

    public SubmitAssessmentCommandHandler(
        IAiModelClient aiClient,
        IAssessmentRepository repository,
        IEventCollector eventCollector,
        ISender sender)
    {
        _aiClient = aiClient;
        _repository = repository;
        _eventCollector = eventCollector;
        _sender = sender;
    }

    public async Task<Result<Guid>> Handle(SubmitAssessmentCommand request, CancellationToken cancellationToken)
    {
        // 1. Call the AI Model
        var payload = new AssessmentSubmissionPayload(request.Answers);

        // This now directly returns the response or throws an exception (caught globally)
        var aiPrediction = await _aiClient.GetPredictionsAsync(payload, cancellationToken);

        var topMatches = aiPrediction.TopMatches.OrderBy(m => m.Rank).ToList();

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
        var savedId = await _repository.AddSubmissionAsync(submission, cancellationToken);

        //5. Publish Integration Event with the mapped integer IDs for Sector and Career    
        var mappingQuery = new GetCareerMappingQuery(topMatches[0].JobLabel);
        var mappingDto = await _sender.Send(mappingQuery, cancellationToken);

        if (mappingDto == null)
        {
            return Result<Guid>.Failure($"Could not map AI label ID '{topMatches[0].JobLabel}' to a valid career.");
        }

        int mappedSectorId = mappingDto.SectorId;
        Guid mappedCareerId = mappingDto.CareerId;

        var integrationEvent = new AssessmentSubmittedIntegrationEvent(
            Guid.NewGuid(),
            DateTime.UtcNow,
            request.UserId,
            mappedSectorId,
            mappedCareerId,
            savedId
        );

        _eventCollector.AddEvent(integrationEvent);

        await _repository.SaveChangesAsync(cancellationToken);
        return Result<Guid>.Success(savedId);
    }
}