namespace CareerPath.Identity.Core.Features.Commands.ChangeRoleToMentor;

using CareerPath.Identity.Core.Contracts;
using CareerPath.Shared.Responses;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

public class ChangeRoleToMentorCommandHandler(
    IUserRepository userRepository,
    IIdentityService identityService) : IRequestHandler<ChangeRoleToMentorCommand, Result>
{
    public async Task<Result> Handle(ChangeRoleToMentorCommand request, CancellationToken cancellationToken)
    {
        // 1. Fetch the user via the repository
        var user = await userRepository.GetByEmailAsync(request.Email, cancellationToken);

        if (user == null)
        {
            // Assuming your Result pattern accepts an Error object or string message for failures
            return Result.Failure(ErrorType.NotFound, "No user found with the provided email address.");
        }

        // 2. Delegate the Microsoft Identity role management to the Infrastructure layer
        var changeRoleResult = await identityService.ChangeUserRoleAsync(user, "Student", "Mentor");

        if (!changeRoleResult.IsSuccess)
        {
            return Result.Failure(changeRoleResult.Error);
        }

        return Result.Success();
    }
}