using CareerPath.Shared.IntegrationEvents.Contracts;
using MediatR;
using System;

namespace CareerPath.Shared.IntegrationEvents.Identity
{
    public record UserRegisteredIntegrationEvent(
        Guid UserId,
        string FirstName,
        string LastName,
        string Role,
        DateTime OccurredOn,
        Guid Id
    ) : IIntegrationEvent;
}