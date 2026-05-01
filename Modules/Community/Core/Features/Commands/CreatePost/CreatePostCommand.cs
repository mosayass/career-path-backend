using CareerPath.Shared.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace CareerPath.Community.Core.Features.Commands.CreatePost
{
    public record CreatePostCommand(
    Guid UserId,
    Guid CommunityId,
    string Title,
    string? Body,
    string CareerTag,
    List<string> MediaUrls) : IRequest<Result<Guid>>;
}
