using CareerPath.Profiles.Api.Contracts.Requests;
using CareerPath.Profiles.Core.Features.Commands.UpdateProfile;
using CareerPath.Profiles.Core.Features.Queries.GetProfileById;
using CareerPath.Shared.Api.Controllers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CareerPath.Profiles.Api.Controllers;


[Authorize] 
public class ProfilesController(ISender sender) : ApiControllerBase(sender)
{
    [HttpGet("me")]
    public async Task<IActionResult> GetMyProfile()
    {
        // Securely extract the UserId from the logged-in user's JWT token claims
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!Guid.TryParse(userIdString, out var userId))
        {
            return Unauthorized();
        }

        // Reuse your existing query, but with the secure ID
        var result = await Sender.Send(new GetProfileByIdQuery(userId));
        return HandleResult(result);
    }
    [HttpGet("{userId:guid}")]
    [AllowAnonymous] // Optional: Remove this attribute if you only want logged-in users to view profiles
    public async Task<IActionResult> GetProfileById(Guid userId)
    {
        var result = await Sender.Send(new GetProfileByIdQuery(userId));
        return HandleResult(result);
    }

    [HttpPut("me")]
    public async Task<IActionResult> UpdateMyProfile([FromBody] UpdateProfileRequest request)
    {
        // Securely extract the UserId from the logged-in user's JWT token claims
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!Guid.TryParse(userIdString, out var userId))
        {
            return Unauthorized();
        }

        // Map the secure UserId and the incoming HTTP body to your MediatR command
        var command = new UpdateProfileCommand(
            userId,
            request.DisplayName,
            request.Bio,
            request.AvatarUrl
        );

        var result = await Sender.Send(command);
        return HandleResult(result);
    }
}