using System;
using System.Collections.Generic;
using CareerPath.Shared.Responses;
using MediatR;

namespace CareerPath.Assessment.Core.Features.Commands.SubmitAssessment;

// We return a Result<Guid> so the API can return the ID of the new assessment submission
public record SubmitAssessmentCommand(
    Guid UserId,
    IEnumerable<float> Answers
) : IRequest<Result<Guid>>;