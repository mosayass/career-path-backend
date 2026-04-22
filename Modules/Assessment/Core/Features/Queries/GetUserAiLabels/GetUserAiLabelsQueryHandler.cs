using CareerPath.Assessment.Core.Contracts;
using CareerPath.Shared.Contracts.Assessment;
using CareerPath.Shared.Responses;
using MediatR;

namespace CareerPath.Assessment.Core.Features.Queries.GetUserAiLabels;

public class GetUserAiLabelsQueryHandler : IRequestHandler<GetUserAiLabelsQuery, Result<UserAiLabelsDto>>
{
    private readonly IAssessmentRepository _repository;

    public GetUserAiLabelsQueryHandler(IAssessmentRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<UserAiLabelsDto>> Handle(GetUserAiLabelsQuery request, CancellationToken cancellationToken)
    {
        var latestSubmission = await _repository.GetSubmissionByIdAsync(request.UserId, cancellationToken);

        if (latestSubmission?.Result == null)
        {
            return Result<UserAiLabelsDto>.Failure("No completed assessment found for this user.");
        }

        var dto = new UserAiLabelsDto(
            PrimaryAiLabelId: latestSubmission.Result.PrimaryJobLabel,
            SecondaryAiLabelIds:
            [
                latestSubmission.Result.SecondaryJobLabel,
                latestSubmission.Result.TertiaryJobLabel
            ]
        );

        return Result<UserAiLabelsDto>.Success(dto);
    }
}