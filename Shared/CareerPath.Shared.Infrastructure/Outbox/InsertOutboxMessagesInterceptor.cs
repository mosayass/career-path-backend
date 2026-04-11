using CareerPath.Shared.IntegrationEvents.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Text.Json;

namespace CareerPath.Shared.Infrastructure.Outbox;

public sealed class InsertOutboxMessagesInterceptor : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        if (eventData.Context is not null)
        {
            InsertOutboxMessages(eventData.Context);
        }

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private void InsertOutboxMessages(DbContext context)
    {
        // 1. Dynamically resolve the Scoped Mailbag via the DbContext
        var eventCollector = context.GetService<IEventCollector>();
        if (eventCollector == null) return;

        // 2. Grab the events
        var events = eventCollector.GetEvents();
        if (!events.Any()) return;

        // 3. Serialize them into Outbox Messages
        var outboxMessages = events.Select(integrationEvent => new OutboxMessage
        {
            Id = Guid.NewGuid(),
            OccurredOn = DateTime.UtcNow,
            Type = integrationEvent.GetType().AssemblyQualifiedName ?? integrationEvent.GetType().Name,
            Content = JsonSerializer.Serialize((object)integrationEvent, integrationEvent.GetType())
        }).ToList();

        // 4. Attach to the current transaction
        context.Set<OutboxMessage>().AddRange(outboxMessages);

        // 5. Empty the bucket so they aren't processed twice
        eventCollector.Clear();
    }
}