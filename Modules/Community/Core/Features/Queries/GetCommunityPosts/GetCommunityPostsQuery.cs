using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace CareerPath.Community.Core.Features.Queries.GetCommunityPosts
{
    // Retrieves the paginated feed for a specific community
    public record GetCommunityPostsQuery(Guid CommunityId, int PageNumber = 1, int PageSize = 20) : IRequest<List<PostDto>>;
}
