using CareerPath.Shared.IntegrationEvents.Contracts;

namespace CareerPath.Shared.Infrastructure.Outbox;

public class EventCollector : IEventCollector
{
    private readonly List<IIntegrationEvent> _events = new();

    public void AddEvent(IIntegrationEvent integrationEvent)
    {
        _events.Add(integrationEvent);
    }

    public IReadOnlyCollection<IIntegrationEvent> GetEvents()
    {
        return _events.AsReadOnly();
    }

    public void Clear()
    {
        _events.Clear();
    }
}