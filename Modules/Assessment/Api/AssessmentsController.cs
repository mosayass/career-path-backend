using CareerPath.Assessment.Core.Features.Commands.SubmitAssessment;
using CareerPath.Assessment.Core.Features.Queries.GetAssessmentResult;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace CareerPath.Assessment.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize] // Enforces that the request must contain a valid JWT
public class AssessmentsController : ControllerBase
{
    private readonly ISender _sender;

    public AssessmentsController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost]
    public async Task<IActionResult> SubmitAssessment(
        [FromBody] SubmitAssessmentRequest request,
        CancellationToken cancellationToken)
    {
        // 1. Securely extract the UserId from the JWT token claims
        // ClaimTypes.NameIdentifier usually corresponds to the "sub" or user ID claim in JWTs
        var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out var userId))
        {
            return Unauthorized(new { Error = "Invalid or missing user identity in token." });
        }

        // 2. Construct the Command using the extracted identity and the request body
        var command = new SubmitAssessmentCommand(userId, request.Answers);

        // 3. Dispatch to the MediatR pipeline (this triggers the Validator, then the Handler)
        var result = await _sender.Send(command, cancellationToken);

        // 4. Return the appropriate HTTP response based on your Result pattern
        if (result.IsSuccess)
        {
            return Ok(new { AssessmentId = result.Value });
        }

        return BadRequest(new { Error = result.Error });
    }
    // Add this using statement at the top:
    // using CareerPath.Assessment.Core.Features.Queries.GetAssessmentResult;

    [HttpGet("{id}")]
    public async Task<IActionResult> GetAssessmentResult(Guid id, CancellationToken cancellationToken)
    {
        // 1. Extract the UserId from the token (just like the POST method)
        var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out var userId))
        {
            return Unauthorized(new { Error = "Invalid or missing user identity in token." });
        }

        // 2. Dispatch the query
        var query = new GetAssessmentResultQuery(userId, id);
        var result = await _sender.Send(query, cancellationToken);

        // 3. Return the payload
        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }

        if (result.Error.Contains("not found", StringComparison.OrdinalIgnoreCase))
            return NotFound(new { Error = result.Error });

        if (result.Error.Contains("permission", StringComparison.OrdinalIgnoreCase))
            return Forbid();

        return BadRequest(new { Error = result.Error });
    }
}

// A lightweight record to map the incoming JSON body
public record SubmitAssessmentRequest(IEnumerable<float> Answers);