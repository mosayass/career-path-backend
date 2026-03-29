using CareerPath.Assessment.Core.Contracts;
using CareerPath.Shared.Contracts.Careers;
using CareerPath.Shared.Responses;
using MediatR;
using CareerPath.Assessment.Core.DTOs;

namespace CareerPath.Assessment.Core.Features.Queries.GetAssessmentResult;

public class GetAssessmentResultQueryHandler : IRequestHandler<GetAssessmentResultQuery, Result<AssessmentResponseDto>>
{
    private readonly IAssessmentRepository _repository;
    private readonly ISender _sender;

    public GetAssessmentResultQueryHandler(IAssessmentRepository repository, ISender sender)
    {
        _repository = repository;
        _sender = sender;
    }

    public async Task<Result<AssessmentResponseDto>> Handle(GetAssessmentResultQuery request, CancellationToken cancellationToken)
    {
        // 1. Fetch from repository, expecting your current Result<AssessmentSubmission> wrapper
        var result = await _repository.GetSubmissionByIdAsync(request.AssessmentId, cancellationToken);

        if (!result.IsSuccess || result.Value is null)
        {
            // Cascade the repository's failure (e.g., NotFound) upward
            return Result<AssessmentResponseDto>.Failure(result.ErrorType, result.Error);
        }

        var submission = result.Value;

        // 2. CRITICAL SECURITY CHECK: Ensure the user requesting this result actually owns it
        if (submission.UserId != request.UserId)
        {
            return Result<AssessmentResponseDto>.Failure(ErrorType.Forbidden, "You do not have permission to view this assessment.");
        }

        if (submission.Result is null)
        {
            return Result<AssessmentResponseDto>.Failure(ErrorType.NotFound, "Assessment results are missing.");
        }

        // 3. Extract the Primary AI Label ID 
        // Note: Assuming your DB entity 'submission.Result' stores the top prediction integer as PrimaryJobLabel
        int aiLabelId = submission.Result.PrimaryJobLabel;
        

        // 4. Dispatch the Cross-Module Query to the Careers module via MediatR
        var careerDetailsResult = await _sender.Send(new GetCareerMatchDetailsQuery(aiLabelId), cancellationToken);

        if (!careerDetailsResult.IsSuccess)
        {
            // 5. If the Careers module couldn't find the ID, cascade the exact failure up
            return Result<AssessmentResponseDto>.Failure(careerDetailsResult.ErrorType, careerDetailsResult.Error);
        }

        var careerData = careerDetailsResult.Value;

        // 6. Map everything into the final Contract-First JSON response
        var finalResponse = new AssessmentResponseDto(
            AssessmentId: submission.Id,
            CompletedAt: submission.SubmittedAt,
            MatchDetails: careerData
        );

        return Result<AssessmentResponseDto>.Success(finalResponse);
    }
}