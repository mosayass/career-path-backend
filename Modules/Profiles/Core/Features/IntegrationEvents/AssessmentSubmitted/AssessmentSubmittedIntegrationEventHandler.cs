using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using CareerPath.Profiles.Core.Contracts;
// Assuming this namespace based on your architectural rules
using CareerPath.Shared.IntegrationEvents.Assessment;

namespace CareerPath.Profiles.Core.Features.IntegrationEvents.AssessmentSubmitted
{
    public class AssessmentSubmittedIntegrationEventHandler : INotificationHandler<AssessmentSubmittedIntegrationEvent>
    {
        private readonly IUserProfileRepository _profileRepository;
        private readonly ILogger<AssessmentSubmittedIntegrationEventHandler> _logger;

        public AssessmentSubmittedIntegrationEventHandler(
            IUserProfileRepository profileRepository,
            ILogger<AssessmentSubmittedIntegrationEventHandler> logger)
        {
            _profileRepository = profileRepository ?? throw new ArgumentNullException(nameof(profileRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task Handle(AssessmentSubmittedIntegrationEvent notification, CancellationToken cancellationToken)
        {
            // 1. Fetch the tracked entity
            var profile = await _profileRepository.GetByIdAsync(notification.UserId, cancellationToken);

            if (profile == null)
            {
                // In a heavily distributed system, the Assessment could finish before the UserRegistered event finishes processing.
                // Logging as an error to track potential race conditions.
                _logger.LogError("Cannot update assessment data. Profile not found for UserId: {UserId}", notification.UserId);
                return;
            }

            // 2. Mutate the state
            profile.PrimarySectorId = notification.PrimarySectorId;
            profile.TargetCareerId = notification.TargetCareerId;
            profile.LatestAssessmentId = notification.AssessmentId;
            profile.UpdatedAt = DateTime.UtcNow;

            // 3. Persist via the new EF Core change-tracking save method
            await _profileRepository.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Successfully updated assessment trajectory for profile {UserId}.", notification.UserId);
        }
    }
}