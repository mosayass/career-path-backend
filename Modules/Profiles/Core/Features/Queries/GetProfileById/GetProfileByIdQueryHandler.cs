using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using CareerPath.Profiles.Core.Contracts;
using CareerPath.Shared.Responses;

namespace CareerPath.Profiles.Core.Features.Queries.GetProfileById
{
    public class GetProfileByIdQueryHandler : IRequestHandler<GetProfileByIdQuery, Result<UserProfileResponse>>
    {
        private readonly IUserProfileRepository _profileRepository;
        private readonly ILogger<GetProfileByIdQueryHandler> _logger;

        public GetProfileByIdQueryHandler(
            IUserProfileRepository profileRepository,
            ILogger<GetProfileByIdQueryHandler> logger)
        {
            _profileRepository = profileRepository ?? throw new ArgumentNullException(nameof(profileRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Result<UserProfileResponse>> Handle(GetProfileByIdQuery request, CancellationToken cancellationToken)
        {
            // 1. Fetch the raw entity (Read-only scenario, no tracking needed, though standard GetByIdAsync is fine here)
            var profile = await _profileRepository.GetByIdAsync(request.UserId, cancellationToken);

            if (profile == null)
            {
                _logger.LogInformation("Profile lookup failed. Not found for UserId: {UserId}", request.UserId);
                return Result<UserProfileResponse>.Failure(ErrorType.NotFound, "The requested profile does not exist.");
            }

            // 2. Map the domain entity to the DTO
            var response = new UserProfileResponse(
                profile.UserId,
                profile.DisplayName,
                profile.Bio,
                profile.AvatarUrl,
                profile.Type.ToString(), // Converting the Enum to a readable string
                profile.PrimarySectorId,
                profile.TargetCareerId,
                profile.LatestAssessmentId,
                profile.ReputationScore,
                profile.IsAcceptingDirectMessages
            );

            // 3. Return the successful result
            return Result<UserProfileResponse>.Success(response);
        }
    }
}