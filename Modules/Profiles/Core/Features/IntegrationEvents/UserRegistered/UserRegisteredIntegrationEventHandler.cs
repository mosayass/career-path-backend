using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using CareerPath.Profiles.Core.Contracts;
using CareerPath.Profiles.Core.Entities;
// Assuming this namespace based on your architectural rules for shared contracts
using CareerPath.Shared.IntegrationEvents.Identity;

namespace CareerPath.Profiles.Core.Features.IntegrationEvents.UserRegistered
{
    public class UserRegisteredIntegrationEventHandler : INotificationHandler<UserRegisteredIntegrationEvent>
    {
        private readonly IUserProfileRepository _profileRepository;
        private readonly ILogger<UserRegisteredIntegrationEventHandler> _logger;

        public UserRegisteredIntegrationEventHandler(
            IUserProfileRepository profileRepository,
            ILogger<UserRegisteredIntegrationEventHandler> logger)
        {
            _profileRepository = profileRepository ?? throw new ArgumentNullException(nameof(profileRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task Handle(UserRegisteredIntegrationEvent notification, CancellationToken cancellationToken)
        {
            // 1. Determine the target ProfileType based on the incoming Identity Role
            // Assuming notification.Role is passed as a string representation of your Roles enum for cross-module decoupling
            ProfileType targetProfileType;

            switch (notification.Role)
            {
                case "Student":
                    targetProfileType = ProfileType.Student;
                    break;

                case "Mentor":
                    targetProfileType = ProfileType.HumanMentor;
                    break;

                case "Admin":
                case "SuperAdmin":
                    _logger.LogInformation("Profile creation skipped for {Role} user {UserId}.", notification.Role, notification.UserId);
                    return; // Exit early; do not create a profile

                default:
                    _logger.LogWarning("Unrecognized role '{Role}' for user {UserId}. Profile creation aborted.", notification.Role, notification.UserId);
                    return;
            }

            // 2. Scaffold the initial Profile entity using the constructor
            var displayName = $"{notification.FirstName} {notification.LastName}".Trim();
            var newProfile = new UserProfile(notification.UserId, displayName, targetProfileType);

            // 3. Persist to the database
            await _profileRepository.AddAsync(newProfile, cancellationToken);

            _logger.LogInformation("Successfully created {ProfileType} profile for user {UserId}.", targetProfileType.ToString(), notification.UserId);
        }
    }
}