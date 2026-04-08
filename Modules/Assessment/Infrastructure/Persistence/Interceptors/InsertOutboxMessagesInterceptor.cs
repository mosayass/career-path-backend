// File: InsertOutboxMessagesInterceptor.cs
using CareerPath.Shared.Domain;
using CareerPath.Shared.IntegrationEvents;
using CareerPath.Shared.IntegrationEvents.Outbox;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Text.Json;

namespace CareerPath.Assessment.Infrastructure.Persistence.Interceptors;

public sealed class InsertOutboxMessagesInterceptor : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        var dbContext = eventData.Context;
        if (dbContext is null) return base.SavingChangesAsync(eventData, result, cancellationToken);

        var outboxMessages = dbContext.ChangeTracker
            .Entries<AggregateRoot>()
            .Select(x => x.Entity)
            .SelectMany(entity =>
            {
                var allEvents = entity.Events.ToList();
                entity.ClearEvents();
                return allEvents;
            })
            // ARCHITECTURAL GATEKEEPER: Only move IIntegrationEvent siblings to the Outbox
            .Where(e => e is IIntegrationEvent)
            .Cast<IIntegrationEvent>()
            .Select(integrationEvent => new OutboxMessage
            {
                Id = integrationEvent.Id,
                OccurredOn = integrationEvent.OccurredOn,
                Type = integrationEvent.GetType().FullName!,
                Content = JsonSerializer.Serialize(integrationEvent, integrationEvent.GetType())
            })
            .ToList();

        if (outboxMessages.Count != 0)
        {
            dbContext.Set<OutboxMessage>().AddRange(outboxMessages);
        }

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}