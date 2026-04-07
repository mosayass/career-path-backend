using System;
using MediatR;

namespace CareerPath.Shared.IntegrationEvents.Identity
{
    public record UserRegisteredIntegrationEvent(
        Guid UserId,
        string FirstName,
        string LastName,
        string Role
    ) : INotification;
}