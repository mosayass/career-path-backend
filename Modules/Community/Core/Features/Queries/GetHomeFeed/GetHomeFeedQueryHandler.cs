using MediatR;
using CareerPath.Shared.Responses;
using CareerPath.Community.Core.Contracts;
using CareerPath.Community.Core.DTOs;

namespace CareerPath.Community.Core.Features.Queries.GetHomeFeed;

public class GetHomeFeedQueryHandler(ICommunityFeedQueries feedQueries) : IRequestHandler<GetHomeFeedQuery, Result<List<PostDto>>>
{
    private readonly ICommunityFeedQueries _feedQueries = feedQueries;

    public async Task<Result<List<PostDto>>> Handle(GetHomeFeedQuery request, CancellationToken cancellationToken)
    {
        var posts = await _feedQueries.GetHomeFeedAsync(
            request.UserId,
            request.PageNumber,
            request.PageSize,
            cancellationToken);

        return Result<List<PostDto>>.Success(posts);
    }
}