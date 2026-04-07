using System;
using MediatR;

namespace CareerPath.Shared.IntegrationEvents.Assessment
{
    public record AssessmentSubmittedIntegrationEvent(
        Guid UserId,
        int PrimarySectorId,
        int TargetCareerId,
        Guid AssessmentId
    ) : INotification;
}