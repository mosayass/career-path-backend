// File: AggregateRoot.cs
using System.Collections.Generic;
using MediatR;

namespace CareerPath.Shared.Domain;

public abstract class AggregateRoot
{
    private readonly List<INotification> _events = new();
    public IReadOnlyCollection<INotification> Events => _events.AsReadOnly();

    public void AddEvent(INotification @event) => _events.Add(@event);
    public void ClearEvents() => _events.Clear();
}