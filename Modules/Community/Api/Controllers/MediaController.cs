using CareerPath.Community.Core.Features.Queries.GetUploadTickets;
using CareerPath.Shared.Api.Controllers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CareerPath.Community.Api.Controllers;

[Authorize]
public class MediaController(ISender sender) : ApiControllerBase(sender)
{
    [HttpPost("upload-tickets")]
    public async Task<IActionResult> GetUploadTickets([FromBody] GetUploadTicketsQuery query, CancellationToken cancellationToken)
    {
        var result = await Sender.Send(query, cancellationToken);
        return HandleResult(result);
    }
}