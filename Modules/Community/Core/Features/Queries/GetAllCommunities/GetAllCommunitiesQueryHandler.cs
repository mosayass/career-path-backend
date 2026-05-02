using MediatR;
using CareerPath.Shared.Responses;
using CareerPath.Community.Core.Contracts;
using CareerPath.Community.Core.DTOs;

namespace CareerPath.Community.Core.Features.Queries.GetAllCommunities;

public class GetAllCommunitiesQueryHandler : IRequestHandler<GetAllCommunitiesQuery, Result<List<CommunityDto>>>
{
    private readonly ICommunityDiscoveryQueries _discoveryQueries;

    public GetAllCommunitiesQueryHandler(ICommunityDiscoveryQueries discoveryQueries)
    {
        _discoveryQueries = discoveryQueries;
    }

    public async Task<Result<List<CommunityDto>>> Handle(GetAllCommunitiesQuery request, CancellationToken cancellationToken)
    {
        var communities = await _discoveryQueries.GetAllCommunitiesAsync(
            request.SearchTerm,
            request.PageNumber,
            request.PageSize,
            cancellationToken);

        return Result<List<CommunityDto>>.Success(communities);
    }
}