using MediatR;
using CareerPath.Shared.Responses;
using CareerPath.Community.Core.Contracts;
using CareerPath.Community.Core.DTOs;

namespace CareerPath.Community.Core.Features.Queries.GetUserCommentVotes;

public class GetUserCommentVotesQueryHandler(ICommunityFeedQueries feedQueries) : IRequestHandler<GetUserCommentVotesQuery, Result<List<CommentVoteStateDto>>>
{
    private readonly ICommunityFeedQueries _feedQueries = feedQueries;

    public async Task<Result<List<CommentVoteStateDto>>> Handle(GetUserCommentVotesQuery request, CancellationToken cancellationToken)
    {
        // 1. Guard clause for empty lists to prevent unnecessary database trips
        if (request.CommentIds == null || request.CommentIds.Count == 0)
        {
            return Result<List<CommentVoteStateDto>>.Success([]);
        }

        // 2. Fetch states
        var voteStates = await _feedQueries.GetUserCommentVotesAsync(
            request.UserId,
            request.CommentIds,
            cancellationToken);

        // 3. Return results
        return Result<List<CommentVoteStateDto>>.Success(voteStates);
    }
}