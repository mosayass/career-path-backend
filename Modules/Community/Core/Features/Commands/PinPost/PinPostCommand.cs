using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace CareerPath.Community.Core.Features.Commands.PinPost
{
    public record PinPostCommand(
    Guid InstructorId,
    Guid PostId,
    Guid CommunityId) : IRequest<bool>;
}
