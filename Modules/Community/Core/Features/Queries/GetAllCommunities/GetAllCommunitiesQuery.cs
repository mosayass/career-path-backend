using MediatR;
using CareerPath.Shared.Responses;
using CareerPath.Community.Core.DTOs;

namespace CareerPath.Community.Core.Features.Queries.GetAllCommunities;

public record GetAllCommunitiesQuery(
    string? SearchTerm = null,
    int PageNumber = 1,
    int PageSize = 20) : IRequest<Result<List<CommunityDto>>>;