using MediatR;
using CareerPath.Shared.Responses;
using CareerPath.Community.Core.Contracts;
using CareerPath.Community.Core.DTOs;
using CareerPath.Shared.Contracts.Assessment; 

namespace CareerPath.Community.Core.Features.Queries.GetSuggestedCommunities;

public class GetSuggestedCommunitiesQueryHandler : IRequestHandler<GetSuggestedCommunitiesQuery, Result<List<CommunityDto>>>
{
    private readonly ISender _sender;
    private readonly ICommunityDiscoveryQueries _discoveryQueries;

    public GetSuggestedCommunitiesQueryHandler(
        ISender sender, 
        ICommunityDiscoveryQueries discoveryQueries)
    {
        _sender = sender;
        _discoveryQueries = discoveryQueries;
    }

    public async Task<Result<List<CommunityDto>>> Handle(GetSuggestedCommunitiesQuery request, CancellationToken cancellationToken)
    {
        // 1. Fetch AI Labels from Assessment Module via MediatR
        var assessmentResult = await _sender.Send(new GetUserAiLabelsQuery(request.UserId), cancellationToken);

        if (assessmentResult == null || assessmentResult.Value == null)
        {
            return Result<List<CommunityDto>>.Failure(ErrorType.NotFound, "No assessment data found for this user.");
        }
        
        // 2. Execute the Discovery Query (Primary/Secondary matching happens in the Infra layer)
        var suggestedCommunities = await _discoveryQueries.GetSuggestedCommunitiesAsync(
            assessmentResult.Value.PrimaryAiLabelId,
           assessmentResult.Value.SecondaryAiLabelIds, 
            cancellationToken);

        // 3. Return results (Order by Primary first)
        return Result<List<CommunityDto>>.Success(
            suggestedCommunities.OrderByDescending(c => c.IsPrimaryMatch).ToList()
        );
    }
}