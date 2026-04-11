using MediatR;
using System;

namespace CareerPath.Shared.IntegrationEvents.Contracts
{
    public interface IIntegrationEvent : INotification
    {
        Guid Id { get; }
        DateTime OccurredOn { get; }
    }
}