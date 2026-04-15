using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace CareerPath.Community.Core.Features.Queries.GetSuggestedCommunities
{
    // Retrieves the AI-matched communities for the student
    public record GetSuggestedCommunitiesQuery(Guid UserId) : IRequest<List<CommunityDto>>;
}
