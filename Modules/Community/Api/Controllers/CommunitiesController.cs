using CareerPath.Community.Core.Features.Commands.JoinCommunity;
using CareerPath.Community.Core.Features.Commands.LeaveCommunity;
using CareerPath.Community.Core.Features.Queries.GetAllCommunities;
using CareerPath.Community.Core.Features.Queries.GetJoinedCommunities;
using CareerPath.Community.Core.Features.Queries.GetSuggestedCommunities;
using CareerPath.Shared.Api.Controllers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace CareerPath.Community.Api.Controllers;

[Authorize]
public class CommunitiesController(ISender sender) : ApiControllerBase(sender)
{
    [HttpGet("suggested/{userId:guid}")]
    public async Task<IActionResult> GetSuggested(Guid userId, CancellationToken cancellationToken)
    {
        var query = new GetSuggestedCommunitiesQuery(userId);
        var result = await Sender.Send(query, cancellationToken);
        return HandleResult(result);
    }

    [HttpGet("joined/{userId:guid}")]
    public async Task<IActionResult> GetJoined(Guid userId, CancellationToken cancellationToken)
    {
        var query = new GetJoinedCommunitiesQuery(userId);
        var result = await Sender.Send(query, cancellationToken);
        return HandleResult(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string? searchTerm, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 20, CancellationToken cancellationToken = default)
    {
        var query = new GetAllCommunitiesQuery(searchTerm, pageNumber, pageSize);
        var result = await Sender.Send(query, cancellationToken);
        return HandleResult(result);
    }

    [HttpPost("{communityId:guid}/join")]
    public async Task<IActionResult> Join(Guid communityId, [FromBody] JoinCommunityCommand command, CancellationToken cancellationToken)
    {
        // Ensure route ID matches payload
        var safeCommand = command with { CommunityId = communityId };

        var result = await Sender.Send(safeCommand, cancellationToken);
        return HandleResult(result, new { Message = "Successfully joined community." });
    }

    [HttpDelete("{communityId:guid}/leave")]
    public async Task<IActionResult> Leave(Guid communityId, [FromBody] LeaveCommunityCommand command, CancellationToken cancellationToken)
    {
        var safeCommand = command with { CommunityId = communityId };

        var result = await Sender.Send(safeCommand, cancellationToken);
        return HandleResult(result, new { Message = "Successfully left community." });
    }
}