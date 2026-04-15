using Careerpath.Community.Core.Enums;
using CareerPath.Shared.Responses;
using MediatR;


namespace CareerPath.Community.Core.Features.Commands.CastVote
{
    public record CastVoteCommand(
    Guid UserId,
    Guid TargetId,
    TargetType TargetType,
    bool IsUpvote) : IRequest<Result<bool>>;
}
