using CareerPath.Community.Core.Enums;
using CareerPath.Shared.Responses;
using MediatR;


namespace CareerPath.Community.Core.Features.Commands.CastPostVote
{
    public record CastPostVoteCommand(
    Guid UserId,
    Guid TargetId,
    bool IsUpvote) : IRequest<Result<bool>>;
}
