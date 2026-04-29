using MediatR;
using CareerPath.Shared.Responses;

namespace CareerPath.Community.Core.Features.Commands.JoinCommunity;

public record JoinCommunityCommand(
    Guid UserId,
    Guid CommunityId) : IRequest<Result<bool>>;