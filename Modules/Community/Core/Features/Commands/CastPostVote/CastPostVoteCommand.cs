using CareerPath.Shared.Responses;
using MediatR;


namespace CareerPath.Community.Core.Features.Commands.CastPostVote
{
    public record CastPostVoteCommand(
    Guid UserId,
    Guid PostId,
    bool IsUpvote) : IRequest<Result<bool>>;
}
