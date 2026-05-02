using MediatR;
using CareerPath.Shared.Responses;

namespace CareerPath.Community.Core.Features.Commands.LeaveCommunity;

public record LeaveCommunityCommand(
    Guid UserId,
    Guid CommunityId) : IRequest<Result<bool>>;