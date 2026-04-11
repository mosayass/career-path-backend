using CareerPath.Shared.IntegrationEvents.Contracts;
using MediatR;
using System;

namespace CareerPath.Shared.IntegrationEvents.Assessment
{
    public record AssessmentSubmittedIntegrationEvent(
        Guid Id,
        DateTime OccurredOn,
        Guid UserId,
        int PrimarySectorId,
        Guid TargetCareerId,
        Guid AssessmentId
    ) : IIntegrationEvent;
}