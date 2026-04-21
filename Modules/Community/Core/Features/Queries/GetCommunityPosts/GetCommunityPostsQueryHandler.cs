using MediatR;
using CareerPath.Shared.Responses;
using CareerPath.Community.Core.Contracts;
using CareerPath.Community.Core.DTOs;

namespace CareerPath.Community.Core.Features.Queries.GetCommunityPosts;

public class GetCommunityPostsQueryHandler : IRequestHandler<GetCommunityPostsQuery, Result<List<PostDto>>>
{
    private readonly ICommunityRepository _communityRepository;
    private readonly ICommunityFeedQueries _feedQueries;

    public GetCommunityPostsQueryHandler(
        ICommunityRepository communityRepository,
        ICommunityFeedQueries feedQueries)
    {
        _communityRepository = communityRepository;
        _feedQueries = feedQueries;
    }

    public async Task<Result<List<PostDto>>> Handle(GetCommunityPostsQuery request, CancellationToken cancellationToken)
    {
        // 1. Lightweight Existence Check via Repository
        var communityExists = await _communityRepository.ExistsAsync(request.CommunityId, cancellationToken);
        if (!communityExists)
        {
            return Result<List<PostDto>>.Failure(ErrorType.NotFound, $"Community with ID {request.CommunityId} does not exist.");
        }

        // 2. Fetch the generic feed securely and asynchronously via the Queries Contract
        var posts = await _feedQueries.GetPostsAsync(request.CommunityId, request.PageNumber, request.PageSize, cancellationToken);

        // 3. Return
        return Result<List<PostDto>>.Success(posts);
    }
}