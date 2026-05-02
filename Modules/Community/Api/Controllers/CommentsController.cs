using CareerPath.Community.Core.Features.Commands.AddComment;
using CareerPath.Community.Core.Features.Commands.CastCommentVote;
using CareerPath.Community.Core.Features.Commands.EndorseComment;
using CareerPath.Community.Core.Features.Commands.RemoveCommentVote;
using CareerPath.Community.Core.Features.Queries.GetUserCommentVotes;
using CareerPath.Shared.Api.Controllers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CareerPath.Community.Api.Controllers;

[Authorize]
public class CommentsController(ISender sender) : ApiControllerBase(sender)
{
    [HttpPost]
    public async Task<IActionResult> AddComment([FromBody] AddCommentCommand command, CancellationToken cancellationToken)
    {
        var result = await Sender.Send(command, cancellationToken);
        return HandleResult(result);
    }

    [HttpPut("{commentId:guid}/endorse")]
    public async Task<IActionResult> Endorse(Guid commentId, [FromBody] EndorseCommentCommand command, CancellationToken cancellationToken)
    {
        var safeCommand = command with { CommentId = commentId };
        var result = await Sender.Send(safeCommand, cancellationToken);
        return HandleResult(result, new { Message = "Comment endorsed successfully." });
    }

    [HttpDelete("{commentId:guid}/votes")]
    public async Task<IActionResult> RemoveVote(Guid commentId, [FromBody] RemoveCommentVoteCommand command, CancellationToken cancellationToken)
    {
        var safeCommand = command with { CommentId = commentId };
        var result = await Sender.Send(safeCommand, cancellationToken);
        return HandleResult(result);
    }

    [HttpPost("votes/user-states")]
    public async Task<IActionResult> GetUserVoteStates([FromBody] GetUserCommentVotesQuery query, CancellationToken cancellationToken)
    {
        var result = await Sender.Send(query, cancellationToken);
        return HandleResult(result);
    }
    [HttpPost("{commentId:guid}/votes")]
    public async Task<IActionResult> CastVote(Guid commentId, [FromBody] CastCommentVoteCommand command, CancellationToken cancellationToken)
    {
        // Force the command to use the secure ID from the URL
        var safeCommand = command with { TargetId = commentId };

        var result = await Sender.Send(safeCommand, cancellationToken);
        return HandleResult(result);
    }
}