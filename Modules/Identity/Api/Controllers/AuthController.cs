using CareerPath.Identity.Core.DTOs;
using CareerPath.Identity.Core.Features.Commands.ForgotPassword;
using CareerPath.Identity.Core.Features.Commands.Register;
using CareerPath.Identity.Core.Features.Commands.ResetPassword;
using CareerPath.Identity.Core.Features.Commands.VerifyEmail;
using CareerPath.Identity.Core.Features.Queries.Login;
using CareerPath.Shared.Api.Controllers;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CareerPath.Identity.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(ISender sender) : ApiControllerBase(sender)
{
    
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequestDto request, CancellationToken cancellationToken)
    {
        var command = new RegisterCommand(request);

        var result = await Sender.Send(command, cancellationToken);

        return HandleResult(result, new { Message = "Registration successful. Check your email for verification." });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto request, CancellationToken cancellationToken)
    {
        var query = new LoginQuery(request);
        var result = await Sender.Send(query, cancellationToken);
        return HandleResult(result);
    }
    [HttpPost("verify-email")]
    public async Task<IActionResult> VerifyEmail([FromBody] VerifyEmailRequestDto request, CancellationToken cancellationToken)
    {
        var command = new VerifyEmailCommand(request);
        var result = await Sender.Send(command, cancellationToken);
        return HandleResult(result, new { Message = "Email successfully verified. You can now log in." });
    }
    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequestDto request, CancellationToken cancellationToken)
    {
        var command = new ForgotPasswordCommand(request.Email);
        var result = await Sender.Send(command, cancellationToken);

        // Security Best Practice: Always return a generic success message here to prevent "Email Enumeration" attacks.
        // The frontend should show this message regardless of whether the email actually existed in the database.
        return HandleResult(result, new { Message = "If an account with that email exists, a password reset link has been sent." });
    }
    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequestDto request, CancellationToken cancellationToken)
    {
        var command = new ResetPasswordCommand(request.Email, request.Token, request.NewPassword);
        var result = await Sender.Send(command, cancellationToken);

        return HandleResult(result, new { Message = "Password has been successfully reset. You can now log in." });
    }
}