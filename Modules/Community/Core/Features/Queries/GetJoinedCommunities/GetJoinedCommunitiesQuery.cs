using MediatR;
using CareerPath.Shared.Responses;
using CareerPath.Community.Core.DTOs;

namespace CareerPath.Community.Core.Features.Queries.GetJoinedCommunities;

public record GetJoinedCommunitiesQuery(Guid UserId) : IRequest<Result<List<CommunityDto>>>;