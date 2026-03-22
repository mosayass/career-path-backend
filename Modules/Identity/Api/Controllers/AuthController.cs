using CareerPath.Identity.Core.DTOs;
using CareerPath.Identity.Core.Features.Commands.Register;
using CareerPath.Identity.Core.Features.Queries.Login;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using CareerPath.Identity.Core.Features.Commands.VerifyEmail;

namespace CareerPath.Identity.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly ISender _sender;

    public AuthController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequestDto request, CancellationToken cancellationToken)
    {
        var command = new RegisterCommand(request);

        var result = await _sender.Send(command, cancellationToken);

        if (!result.IsSuccess)
            return BadRequest(new { Error = result.Error }); // Returns a 400 Bad Request if the credentials are wrong or user is not found


        // Returns a 200 OK for successful registration
        return Ok(new { Message = "Registration successful." });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto request, CancellationToken cancellationToken)
    {
        var query = new LoginQuery(request);

        var result = await _sender.Send(query, cancellationToken);

        if (!result.IsSuccess)
            return BadRequest(new { Error = result.Error }); // Returns a 400 Bad Request if the credentials are wrong or user is not found

        // Returns a 200 OK with the generated JWT string from the Result<T>.Value
        return Ok(new { Token = result.Value });
    }
    [HttpPost("verify-email")]
    public async Task<IActionResult> VerifyEmail([FromBody] VerifyEmailRequestDto request, CancellationToken cancellationToken)
    {
        var command = new VerifyEmailCommand(request);

        var result = await _sender.Send(command, cancellationToken);

        // Using !result.IsSuccess based on our previous Result pattern discussion
        if (!result.IsSuccess)
        {
            return BadRequest(new { Error = result.Error });
        }

        return Ok(new { Message = "Email successfully verified. You can now log in." });
    }
}