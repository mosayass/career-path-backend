using MediatR;

namespace CareerPath.Community.Core.Features.Commands.PinPost
{
    public record PinPostCommand(
    Guid InstructorId,
    Guid PostId,
    Guid CommunityId) : IRequest<bool>;
}
