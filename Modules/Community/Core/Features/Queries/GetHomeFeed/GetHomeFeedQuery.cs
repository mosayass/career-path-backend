using MediatR;
using CareerPath.Shared.Responses;
using CareerPath.Community.Core.DTOs;

namespace CareerPath.Community.Core.Features.Queries.GetHomeFeed;

public record GetHomeFeedQuery(
    Guid UserId,
    int PageNumber = 1,
    int PageSize = 20) : IRequest<Result<List<PostDto>>>;