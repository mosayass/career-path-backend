using CareerPath.Profiles.Core.Contracts;
using CareerPath.Shared.Contracts.Profiles;
using CareerPath.Shared.Responses;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace CareerPath.Profiles.Core.Features.Queries.GetProfileData;
// This handler is responsible for returning desplay name and avatarURL it will be used by another module, fetching the profile data from the repository, and mapping it to the ProfileDataResponse DTO.
public class GetProfileDataQueryHandler(IUserProfileRepository profileRepository)
    : IRequestHandler<GetProfileDataQuery, Result<ProfileDataResponse>>
{
    public async Task<Result<ProfileDataResponse>> Handle(GetProfileDataQuery request, CancellationToken cancellationToken)
    {
        // 1. Repository remains pure: fetches the Domain Entity
        var profile = await profileRepository.GetByIdAsync(request.UserId, cancellationToken);

        if (profile == null)
        {
            return Result<ProfileDataResponse>.Failure(
                ErrorType.NotFound,
                $"Profile for User ID {request.UserId} was not found.");
        }

        // 2. Map to the Shared DTO. 
        // Note: Adjust 'FirstName' / 'LastName' or 'DisplayName' based on your exact Entity properties.
        var dto = new ProfileDataResponse(
            UserId: profile.UserId,
            DisplayName: profile.DisplayName, // Or  if you have that
            AvatarUrl: profile.AvatarUrl
        );

        return Result<ProfileDataResponse>.Success(dto);
    }
}