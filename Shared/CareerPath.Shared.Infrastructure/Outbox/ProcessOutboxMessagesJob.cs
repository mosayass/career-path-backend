using System;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using CareerPath.Shared.IntegrationEvents.Contracts;

namespace CareerPath.Shared.Infrastructure.Outbox;

public class ProcessOutboxMessagesJob<TDbContext> : BackgroundService
    where TDbContext : DbContext
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<ProcessOutboxMessagesJob<TDbContext>> _logger;

    public ProcessOutboxMessagesJob(
        IServiceProvider serviceProvider,
        ILogger<ProcessOutboxMessagesJob<TDbContext>> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                // Dynamically resolve whichever DbContext was passed in the generic type
                var dbContext = scope.ServiceProvider.GetRequiredService<TDbContext>();
                var publisher = scope.ServiceProvider.GetRequiredService<IPublisher>();

                // Dynamically access the OutboxMessages table for this specific context
                var outboxMessages = await dbContext.Set<OutboxMessage>()
                    .Where(m => m.ProcessedOn == null)
                    .OrderBy(m => m.OccurredOn)
                    .Take(20)
                    .ToListAsync(stoppingToken);

                if (outboxMessages.Any())
                {
                    foreach (var message in outboxMessages)
                    {
                        var eventType = Type.GetType(message.Type);
                        if (eventType == null)
                        {
                            _logger.LogWarning("Could not resolve type {Type}", message.Type);
                            continue;
                        }

                        var integrationEvent = JsonSerializer.Deserialize(message.Content, eventType) as IIntegrationEvent;

                        if (integrationEvent is not null)
                        {
                            // Broadcast to MediatR
                            await publisher.Publish(integrationEvent, stoppingToken);
                        }

                        message.ProcessedOn = DateTime.UtcNow;
                    }

                    await dbContext.SaveChangesAsync(stoppingToken);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing outbox messages for {DbContextContext}", typeof(TDbContext).Name);
            }

            await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
        }
    }
}