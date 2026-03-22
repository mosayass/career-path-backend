using CareerPath.Identity.Core.Contracts;
using CareerPath.Shared.Responses;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace CareerPath.Identity.Core.Features.Commands.VerifyEmail;

public class VerifyEmailCommandHandler : IRequestHandler<VerifyEmailCommand, Result>
{
    private readonly IUserRepository _userRepository;

    public VerifyEmailCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Result> Handle(VerifyEmailCommand request, CancellationToken cancellationToken)
    {
        // 1. Find the user
        var user = await _userRepository.GetByEmailAsync(request.RequestDto.Email, cancellationToken);
        if (user == null)
        {
            return Result.Failure("User not found.");
        }

        // 2. Check if they are already verified
        if (user.EmailConfirmed)
        {
            return Result.Failure("Email is already confirmed.");
        }

        // 3. Verify the 6-digit OTP matches what is in the database
        bool isValid = await _userRepository.VerifyOtpAsync(user, "CareerPathApp", "EmailConfirmationOTP", request.RequestDto.Code, cancellationToken);
        if (!isValid)
        {
            return Result.Failure("Invalid or expired verification code.");
        }

        // 4. Update their status to Confirmed
        var confirmationResult = await _userRepository.ConfirmEmailAsync(user, cancellationToken);
        if (!confirmationResult.IsSuccess)
        {
            return confirmationResult;
        }

        return Result.Success();
    }
}