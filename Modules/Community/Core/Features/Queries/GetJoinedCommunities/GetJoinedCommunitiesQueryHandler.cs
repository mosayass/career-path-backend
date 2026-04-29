using MediatR;
using CareerPath.Shared.Responses;
using CareerPath.Community.Core.Contracts;
using CareerPath.Community.Core.DTOs;

namespace CareerPath.Community.Core.Features.Queries.GetJoinedCommunities;

public class GetJoinedCommunitiesQueryHandler : IRequestHandler<GetJoinedCommunitiesQuery, Result<List<CommunityDto>>>
{
    private readonly ICommunityDiscoveryQueries _discoveryQueries;

    public GetJoinedCommunitiesQueryHandler(ICommunityDiscoveryQueries discoveryQueries)
    {
        _discoveryQueries = discoveryQueries;
    }

    public async Task<Result<List<CommunityDto>>> Handle(GetJoinedCommunitiesQuery request, CancellationToken cancellationToken)
    {
        var joinedCommunities = await _discoveryQueries.GetJoinedCommunitiesAsync(request.UserId, cancellationToken);

        return Result<List<CommunityDto>>.Success(joinedCommunities);
    }
}