using MediatR;
using Microsoft.AspNetCore.Mvc;
using CareerPath.Shared.Api.Controllers;
using CareerPath.Community.Core.Features.Commands.CreatePost;
using CareerPath.Community.Core.Features.Commands.PinPost;
using CareerPath.Community.Core.Features.Commands.CastPostVote;
using CareerPath.Community.Core.Features.Commands.RemovePostVote;
using CareerPath.Community.Core.Features.Queries.GetCommunityPosts;
using CareerPath.Community.Core.Features.Queries.GetHomeFeed;
using CareerPath.Community.Core.Features.Queries.GetPostWithComments;
using CareerPath.Community.Core.Features.Queries.GetUserPostVotes;

namespace CareerPath.Api.Controllers;

public class PostsController : ApiControllerBase
{
    public PostsController(ISender sender) : base(sender) { }

    [HttpPost]
    public async Task<IActionResult> CreatePost([FromBody] CreatePostCommand command, CancellationToken cancellationToken)
    {
        var result = await Sender.Send(command, cancellationToken);
        return HandleResult(result);
    }

    [HttpGet("community/{communityId:guid}")]
    public async Task<IActionResult> GetCommunityFeed(Guid communityId, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 20, CancellationToken cancellationToken = default)
    {
        var query = new GetCommunityPostsQuery(communityId, pageNumber, pageSize);
        var result = await Sender.Send(query, cancellationToken);
        return HandleResult(result);
    }

    [HttpGet("home/{userId:guid}")]
    public async Task<IActionResult> GetHomeFeed(Guid userId, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 20, CancellationToken cancellationToken = default)
    {
        var query = new GetHomeFeedQuery(userId, pageNumber, pageSize);
        var result = await Sender.Send(query, cancellationToken);
        return HandleResult(result);
    }

    [HttpGet("{postId:guid}")]
    public async Task<IActionResult> GetPostDetails(Guid postId, CancellationToken cancellationToken)
    {
        var query = new GetPostWithCommentsQuery(postId);
        var result = await Sender.Send(query, cancellationToken);
        return HandleResult(result);
    }

    [HttpPut("{postId:guid}/pin")]
    public async Task<IActionResult> PinPost(Guid postId, [FromBody] PinPostCommand command, CancellationToken cancellationToken)
    {
        var safeCommand = command with { PostId = postId };
        var result = await Sender.Send(safeCommand, cancellationToken);
        return HandleResult(result, new { Message = "Post pinned successfully." });
    }

    [HttpPost("votes")]
    public async Task<IActionResult> CastVote([FromBody] CastPostVoteCommand command, CancellationToken cancellationToken)
    {
        var result = await Sender.Send(command, cancellationToken);
        return HandleResult(result);
    }

    [HttpDelete("{postId:guid}/votes")]
    public async Task<IActionResult> RemoveVote(Guid postId, [FromBody] RemovePostVoteCommand command, CancellationToken cancellationToken)
    {
        var safeCommand = command with { PostId = postId };
        var result = await Sender.Send(safeCommand, cancellationToken);
        return HandleResult(result);
    }

    [HttpPost("votes/user-states")]
    public async Task<IActionResult> GetUserVoteStates([FromBody] GetUserPostVotesQuery query, CancellationToken cancellationToken)
    {
        var result = await Sender.Send(query, cancellationToken);
        return HandleResult(result);
    }
}