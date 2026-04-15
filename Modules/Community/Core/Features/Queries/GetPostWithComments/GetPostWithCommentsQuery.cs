using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace CareerPath.Community.Core.Features.Queries.GetPostWithComments
{
    // Retrieves the full post with its nested comment thread
    public record GetPostWithCommentsQuery(Guid PostId) : IRequest<PostDetailsDto>;
}
