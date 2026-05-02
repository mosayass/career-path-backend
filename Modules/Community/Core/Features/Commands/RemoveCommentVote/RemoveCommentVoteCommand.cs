using MediatR;
using CareerPath.Shared.Responses;

namespace CareerPath.Community.Core.Features.Commands.RemoveCommentVote;

public record RemoveCommentVoteCommand(
    Guid UserId,
    Guid CommentId) : IRequest<Result<bool>>;