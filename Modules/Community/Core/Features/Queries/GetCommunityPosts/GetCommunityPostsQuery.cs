using CareerPath.Community.Core.DTOs;
using MediatR;
using CareerPath.Shared.Responses;

namespace CareerPath.Community.Core.Features.Queries.GetCommunityPosts
{
    // Retrieves the paginated feed for a specific community
    public record GetCommunityPostsQuery(
        Guid CommunityId,
        int PageNumber = 1,
        int PageSize = 20) : IRequest<Result<List<PostDto>>>;
}
 