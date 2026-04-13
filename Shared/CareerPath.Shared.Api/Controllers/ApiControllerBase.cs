using CareerPath.Shared.Responses;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CareerPath.Shared.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public abstract class ApiControllerBase : ControllerBase
{
    protected readonly ISender Sender;

    protected ApiControllerBase(ISender sender)
    {
        Sender = sender;
    }

    // 1. For Queries that return data (Result<T>)
    protected IActionResult HandleResult<T>(Result<T> result)
    {
        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }

        return MapErrorToStatusCode(result.ErrorType, result.Error);
    }

    // For Commands that do not return data (Result)
    // Includes an optional payload so you can return custom success messages
    protected IActionResult HandleResult(Result result, object? successPayload = null)
    {
        if (result.IsSuccess)
        {
            return successPayload is not null ? Ok(successPayload) : NoContent();
        }

        return MapErrorToStatusCode(result.ErrorType, result.Error);
    }

    //Extracted private method to keep the error logic DRY
    private IActionResult MapErrorToStatusCode(ErrorType errorType, string error)
    {
        return errorType switch
        {
            ErrorType.NotFound => NotFound(error),
            ErrorType.Validation => BadRequest(error),
            ErrorType.Conflict => Conflict(error),
            ErrorType.Unauthorized => Unauthorized(error),
            ErrorType.Forbidden => Forbid(),
            _ => StatusCode(StatusCodes.Status500InternalServerError, error)
        };
    }
}