using System.Collections.Generic;

namespace CareerPath.Shared.IntegrationEvents.Contracts;

public interface IEventCollector
{
    void AddEvent(IIntegrationEvent integrationEvent);
    IReadOnlyCollection<IIntegrationEvent> GetEvents();
    void Clear();
}