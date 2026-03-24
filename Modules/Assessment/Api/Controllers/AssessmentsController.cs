using CareerPath.Shared.Extensions;
using CareerPath.Assessment.Core.Features.Commands.SubmitAssessment;
using CareerPath.Assessment.Core.Features.Queries.GetAssessmentResult;
using CareerPath.Shared.Responses;
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
        var userId = User.GetUserId();
        if (userId == null) return Unauthorized(new { Error = "Invalid or missing user identity in token." });



        // 2. Construct the Command using the extracted identity and the request body
        var command = new SubmitAssessmentCommand(userId.Value, request.Answers);

        // 3. Dispatch to the MediatR pipeline (this triggers the Validator, then the Handler)
        var result = await _sender.Send(command, cancellationToken);

        // 4. Return the appropriate HTTP response based on your Result pattern
        if (result.IsSuccess) return Ok(new { AssessmentId = result.Value });

        return result.ErrorType switch
        {
            ErrorType.NotFound => NotFound(new { Error = result.Error }),
            ErrorType.Forbidden => Forbid(),
            ErrorType.Conflict => Conflict(new { Error = result.Error }),
            _ => BadRequest(new { Error = result.Error })
        };
    }
    // Add this using statement at the top:
    // using CareerPath.Assessment.Core.Features.Queries.GetAssessmentResult;

    [HttpGet("{id}")]
    public async Task<IActionResult> GetAssessmentResult(Guid id, CancellationToken cancellationToken)
    {
        // 1. Extract the UserId from the token (just like the POST method)
        var userId = User.GetUserId();
        if (userId == null) return Unauthorized(new { Error = "Invalid or missing user identity in token." });
        // 2. Dispatch the query
        var query = new GetAssessmentResultQuery(userId.Value, id);
        var result = await _sender.Send(query, cancellationToken);

        // 3. Return the payload
        if (result.IsSuccess) return Ok(result.Value);

        return result.ErrorType switch
        {
            ErrorType.NotFound => NotFound(new { Error = result.Error }),
            ErrorType.Forbidden => Forbid(),
            ErrorType.Conflict => Conflict(new { Error = result.Error }),
            _ => BadRequest(new { Error = result.Error })
        };
    }

}
// A lightweight record to map the incoming JSON body
public record SubmitAssessmentRequest(IEnumerable<float> Answers);