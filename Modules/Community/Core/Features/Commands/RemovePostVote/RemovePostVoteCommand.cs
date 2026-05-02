using MediatR;
using CareerPath.Shared.Responses;

namespace CareerPath.Community.Core.Features.Commands.RemovePostVote;

public record RemovePostVoteCommand(
    Guid PostId,
    Guid UserId) : IRequest<Result<bool>>;