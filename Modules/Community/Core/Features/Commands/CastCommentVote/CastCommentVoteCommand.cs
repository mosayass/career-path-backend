using CareerPath.Community.Core.Enums;
using CareerPath.Shared.Responses;
using MediatR;

namespace CareerPath.Community.Core.Features.Commands.CastCommentVote
{
    public record CastCommentVoteCommand(
        Guid UserId,
        Guid TargetId,
        bool IsUpvote) : IRequest<Result<bool>>;
}