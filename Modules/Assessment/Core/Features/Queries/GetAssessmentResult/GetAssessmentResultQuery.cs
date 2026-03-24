using System;
using CareerPath.Assessment.Core.DTOs;
using CareerPath.Shared.Responses;
using MediatR;

namespace CareerPath.Assessment.Core.Features.Queries.GetAssessmentResult;

public record GetAssessmentResultQuery(
    Guid UserId,
    Guid AssessmentId
) : IRequest<Result<AssessmentResultResponse>>;