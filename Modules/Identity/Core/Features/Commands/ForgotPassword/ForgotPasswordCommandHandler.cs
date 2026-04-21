using CareerPath.Identity.Core.Contracts;
using CareerPath.Shared.Responses;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace CareerPath.Identity.Core.Features.Commands.ForgotPassword;

public class ForgotPasswordCommandHandler(
    IUserRepository userRepository,
    IIdentityService identityService,
    IEmailService emailService) : IRequestHandler<ForgotPasswordCommand, Result>
{
    public async Task<Result> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByEmailAsync(request.Email, cancellationToken);

        // Business Rule: If user is null, we STILL return Success to prevent email enumeration attacks.
        if (user == null)
        {
            return Result.Success();
        }

        var token = await identityService.GeneratePasswordResetTokenAsync(user);
       
        // Orchestration: Construct the simulated React frontend link
        var resetLink = $"http://localhost:3000/reset-password?email={user.Email}&token={token}";
        var emailBody = $"You requested a password reset. Click the link below to reset your password:\n\n{resetLink}\n\nIf you did not request this, please ignore this email.";

        await emailService.SendEmailAsync(user.Email, "CPath - Password Reset Request", emailBody);

        return Result.Success();
    }
}