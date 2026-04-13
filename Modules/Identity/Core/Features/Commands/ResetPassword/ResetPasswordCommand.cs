using CareerPath.Shared.Responses;
using MediatR;

namespace CareerPath.Identity.Core.Features.Commands.ResetPassword;

// The intent to finalize the password reset with the provided token.
public record ResetPasswordCommand(
    string Email,
    string Token,
    string NewPassword
    ) : IRequest<Result>;