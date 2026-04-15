using CareerPath.Shared.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace CareerPath.Community.Core.Features.Commands.EndorseComment
{
    public record EndorseCommentCommand(
    Guid InstructorId,
    Guid CommentId,
    Guid CommunityId) : IRequest<Result<bool>>;
}
