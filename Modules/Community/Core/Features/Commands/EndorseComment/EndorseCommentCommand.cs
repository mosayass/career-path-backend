using CareerPath.Shared.Responses;
using MediatR;


namespace CareerPath.Community.Core.Features.Commands.EndorseComment
{
    public record EndorseCommentCommand(
    Guid InstructorId,
    Guid CommentId,
    Guid CommunityId) : IRequest<Result<bool>>;
}
