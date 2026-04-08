// File: OutboxMessage.cs
// Location: careerpath.shared.integrationevents (or a dedicated Outbox folder within it)

using System;

namespace CareerPath.Shared.IntegrationEvents.Outbox
{
    public sealed class OutboxMessage
    {
        public Guid Id { get; set; }

        // The fully qualified assembly name of the event type (e.g., "CareerPath.Shared.IntegrationEvents.Assessment.AssessmentSubmittedIntegrationEvent")
        public string Type { get; set; } = string.Empty;

        // The serialized JSON payload of the event
        public string Content { get; set; } = string.Empty;

        public DateTime OccurredOn { get; set; }

        public DateTime? ProcessedOn { get; set; }

        // Optional but highly recommended: stores exception details if the background worker fails to publish
        public string? Error { get; set; }
    }
}