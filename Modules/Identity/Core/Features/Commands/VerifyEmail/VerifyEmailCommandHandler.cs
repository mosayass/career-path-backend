using CareerPath.Identity.Core.Contracts;
using CareerPath.Shared.IntegrationEvents.Contracts;
using CareerPath.Shared.IntegrationEvents.Identity;
using CareerPath.Shared.Responses;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CareerPath.Identity.Core.Features.Commands.VerifyEmail;

public class VerifyEmailCommandHandler : IRequestHandler<VerifyEmailCommand, Result>
{
    private readonly IUserRepository _userRepository;
    private readonly IEventCollector _eventCollector;
    public VerifyEmailCommandHandler(IUserRepository userRepository, IEventCollector eventCollector)
    {
        _userRepository = userRepository;
        _eventCollector = eventCollector;
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
        //TODO UPDATE THIS TO MAP FROM USER ENTITY
        var integrationEvent = new UserRegisteredIntegrationEvent(
            Id: Guid.NewGuid(),
            OccurredOn: DateTime.UtcNow,
            UserId: user.Id,
            FirstName: user.FirstName ?? "Unknown", // Map from your user entity
            LastName: user.LastName ?? "Unknown",   // Map from your user entity
            Role: "User" // Or map from user.Role if you have it
        );
        _eventCollector.AddEvent(integrationEvent);

        // 4. Update their status to Confirmed
        await _userRepository.ConfirmEmailAsync(user, cancellationToken);

        return Result.Success();
    }
}