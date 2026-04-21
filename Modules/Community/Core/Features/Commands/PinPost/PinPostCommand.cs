using CareerPath.Shared.Responses;
using MediatR;

namespace CareerPath.Community.Core.Features.Commands.PinPost
{
    public record PinPostCommand(
    Guid UserId,
    Guid PostId,
    Guid CommunityId) : IRequest<Result<bool>>;
}
