namespace CareerPath.Community.Core.Features.Queries.GetUserPostVotes;

public record PostVoteStateDto(
    Guid PostId,
    bool IsUpvote);