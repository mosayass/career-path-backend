using CareerPath.Shared.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace CareerPath.Community.Core.Features.Commands.AddComment
{
    public record AddCommentCommand(
    Guid UserId,
    Guid PostId,
    Guid? ParentCommentId,
    string Body) : IRequest<Result<Guid>>;
}
