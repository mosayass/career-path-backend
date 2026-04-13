using CareerPath.Identity.Core.Contracts;
using CareerPath.Shared.Responses;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CareerPath.Identity.Core.Features.Commands.ResetPassword;

public class ResetPasswordCommandHandler(
    IUserRepository userRepository,
    IIdentityService identityService)
    : IRequestHandler<ResetPasswordCommand, Result>
{
    public async Task<Result> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByEmailAsync(request.Email, cancellationToken);

        if (user == null)
        {
            return Result.Failure(ErrorType.NotFound, "User not found.");
        }
        var (succeeded, errorMessage) = await identityService.ResetPasswordAsync(user, request.Token, request.NewPassword);

        if (!succeeded)
        {
            return Result.Failure(ErrorType.Validation, errorMessage ?? "Failed to reset password.");
        }

        return Result.Success();
        
    }
}