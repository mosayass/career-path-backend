using CareerPath.Shared.Responses; // Your Result pattern
using MediatR;
using Microsoft.AspNetCore.Http; // Needed for StatusCodes
using Microsoft.AspNetCore.Mvc;

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

    protected IActionResult HandleResult<T>(Result<T> result)
    {
        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }

        return result.ErrorType switch
        {
            ErrorType.NotFound => NotFound(result.Error),
            ErrorType.Validation => BadRequest(result.Error),
            ErrorType.Conflict => Conflict(result.Error),
            ErrorType.Unauthorized => Unauthorized(result.Error),
            ErrorType.Forbidden => Forbid(),
            _ => StatusCode(StatusCodes.Status500InternalServerError, result.Error)
        };
    }
}