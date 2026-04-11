using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using CareerPath.Profiles.Core.Contracts;
using CareerPath.Shared.Responses;

namespace CareerPath.Profiles.Core.Features.Commands.UpdateProfile
{
    public class UpdateProfileCommandHandler : IRequestHandler<UpdateProfileCommand, Result<Unit>>
    {
        private readonly IUserProfileRepository _profileRepository;
        private readonly ILogger<UpdateProfileCommandHandler> _logger;

        public UpdateProfileCommandHandler(
            IUserProfileRepository profileRepository,
            ILogger<UpdateProfileCommandHandler> logger)
        {
            _profileRepository = profileRepository ?? throw new ArgumentNullException(nameof(profileRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Result<Unit>> Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
        {
            // 1. Fetch the tracked entity
            var profile = await _profileRepository.GetByIdAsync(request.UserId, cancellationToken);

            if (profile == null)
            {
                _logger.LogWarning("Manual update failed. Profile not found for UserId: {UserId}", request.UserId);
                // Note: Adjust the Failure parameters to match your exact Result<T> implementation
                return Result<Unit>.Failure(ErrorType.NotFound, "The requested profile does not exist.");
            }

            // 2. Mutate the specific state fields
            profile.DisplayName = request.DisplayName;
            profile.Bio = request.Bio;
            profile.AvatarUrl = request.AvatarUrl;
            profile.UpdatedAt = DateTime.UtcNow;

            // 3. Persist via the EF Core change-tracking save method
            await _profileRepository.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Successfully updated profile details for UserId: {UserId}.", request.UserId);

            return Result<Unit>.Success(Unit.Value);
        }
    }
}