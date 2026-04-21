using MediatR;
using CareerPath.Shared.Responses;
using CareerPath.Community.Core.Contracts;

namespace CareerPath.Community.Core.Features.Queries.GetUserPostVotes;

public class GetUserPostVotesQueryHandler : IRequestHandler<GetUserPostVotesQuery, Result<List<PostVoteStateDto>>>
{
    private readonly ICommunityFeedQueries _feedQueries;

    public GetUserPostVotesQueryHandler(ICommunityFeedQueries feedQueries)
    {
        _feedQueries = feedQueries;
    }

    public async Task<Result<List<PostVoteStateDto>>> Handle(GetUserPostVotesQuery request, CancellationToken cancellationToken)
    {
        if (request.PostIds == null || !request.PostIds.Any())
        {
            return Result<List<PostVoteStateDto>>.Success(new List<PostVoteStateDto>());
        }

        // Asynchronously fetch the lightweight state array
        var userVotes = await _feedQueries.GetUserVotesAsync(request.UserId, request.PostIds, cancellationToken);

        return Result<List<PostVoteStateDto>>.Success(userVotes);
    }
}