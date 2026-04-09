// File: ProcessOutboxMessagesJob.cs
// Location: CareerPath.Assessment.Infrastructure.BackgroundJobs

using System.Text.Json;
using CareerPath.Shared.IntegrationEvents;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using CareerPath.Assessment.Infrastructure.Persistence;

namespace CareerPath.Assessment.Infrastructure.BackgroundJobs;

public sealed class ProcessOutboxMessagesJob : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<ProcessOutboxMessagesJob> _logger;

    public ProcessOutboxMessagesJob(IServiceScopeFactory scopeFactory, ILogger<ProcessOutboxMessagesJob> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _scopeFactory.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<AssessmentsDbContext>();
                var publisher = scope.ServiceProvider.GetRequiredService<IPublisher>();

                // 1. Fetch unprocessed messages
                var messages = await dbContext.OutboxMessages
                    .Where(m => m.ProcessedOn == null)
                    .OrderBy(m => m.OccurredOn)
                    .Take(20)
                    .ToListAsync(stoppingToken);

                foreach (var message in messages)
                {
                    try
                    {
                        // 2. Deserialize the Fat Event back to its specific type
                        var eventType = Type.GetType(message.Type);
                        if (eventType == null) continue;

                        var integrationEvent = JsonSerializer.Deserialize(message.Content, eventType) as INotification;

                        if (integrationEvent != null)
                        {
                            // 3. Publish via MediatR
                            await publisher.Publish(integrationEvent, stoppingToken);
                        }

                        message.ProcessedOn = DateTime.UtcNow;
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Failed to process outbox message {MessageId}", message.Id);
                        message.Error = ex.Message;
                    }
                }

                await dbContext.SaveChangesAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Critical error in Outbox Background Job");
            }

            // 4. Wait before polling again 
            // chage it to much longer in production, this is just for demo purposes to see the job working more quickly
            await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
        }
    }
}