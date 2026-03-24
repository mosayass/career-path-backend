using System.Threading;
using System.Threading.Tasks;
using CareerPath.Assessment.Core.Contracts;
using CareerPath.Assessment.Core.DTOs;
using CareerPath.Shared.Responses;
using MediatR;

namespace CareerPath.Assessment.Core.Features.Queries.GetAssessmentResult;

public class GetAssessmentResultQueryHandler : IRequestHandler<GetAssessmentResultQuery, Result<AssessmentResultResponse>>
{
    private readonly IAssessmentRepository _repository;

    public GetAssessmentResultQueryHandler(IAssessmentRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<AssessmentResultResponse>> Handle(GetAssessmentResultQuery request, CancellationToken cancellationToken)
    {
        var result = await _repository.GetSubmissionByIdAsync(request.AssessmentId, cancellationToken);

        if (!result.IsSuccess || result.Value == null)
            return Result<AssessmentResultResponse>.Failure(result.Error);

        var submission = result.Value;

        // CRITICAL SECURITY CHECK: Ensure the user requesting this result actually owns it
        if (submission.UserId != request.UserId)
            return Result<AssessmentResultResponse>.Failure("You do not have permission to view this assessment.");

        if (submission.Result == null)
            return Result<AssessmentResultResponse>.Failure("Assessment results are missing.");

        // Map to our clean DTO
        var response = new AssessmentResultResponse(
            submission.Id,
            submission.SubmittedAt,
            submission.Result.PrimaryCareer,
            submission.Result.PrimaryConfidence,
            submission.Result.SecondaryCareer,
            submission.Result.SecondaryConfidence,
            submission.Result.TertiaryCareer,
            submission.Result.TertiaryConfidence
        );

        return Result<AssessmentResultResponse>.Success(response);
    }
}