using CareerPath.Community.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace CareerPath.Community.Core.Features.Queries.GetPostWithComments
{
    public record PostDetailsDto(
      PostDto Post,
      List<CommentDto> Comments); // Level 1 comments sorted by votes
}
