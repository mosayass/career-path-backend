using MediatR;
using System;

namespace CareerPath.Shared.IntegrationEvents
{
    public interface IIntegrationEvent : INotification
    {
        Guid Id { get; }
        DateTime OccurredOn { get; }
    }
}