using CareerPath.Community.Core.DTOs;
using CareerPath.Community.Core.Features.Queries.GetUserPostVotes;

namespace CareerPath.Community.Core.Contracts;

public interface ICommunityFeedQueries
{
    Task<List<PostDto>> GetPostsAsync(Guid communityId, int pageNumber, int pageSize, CancellationToken cancellationToken);
    Task<List<PostVoteStateDto>> GetUserVotesAsync(Guid userId, List<Guid> postIds, CancellationToken cancellationToken);
    Task<List<PostDto>> GetHomeFeedAsync(
        Guid userId,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken);
    Task<List<CommentVoteStateDto>> GetUserCommentVotesAsync(
        Guid userId,
        List<Guid> commentIds,
        CancellationToken cancellationToken);
}