using CareerPath.Identity.Core.Contracts;
using CareerPath.Identity.Core.DTOs;
using CareerPath.Identity.Core.Entities;
using CareerPath.Identity.Core.Enums;
using CareerPath.Shared.Responses;
using MediatR;

namespace CareerPath.Identity.Core.Features.Commands.Register;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, Result>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IEmailService _emailService;

    public RegisterCommandHandler(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IEmailService emailService)
    {
        _userRepository = userRepository ;
        _passwordHasher = passwordHasher;
        _emailService = emailService;
    }

    public async Task<Result> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var dto = request.RequestDto;

        // 1. Check if user already exists (Using YOUR original method)
        var existingUser = await _userRepository.GetByEmailAsync(dto.Email, cancellationToken);
        if (existingUser != null)
            return Result.Failure("A user with this email already exists.");

        // 2. Hash the password securely
        var hashedPassword = _passwordHasher.Hash(dto.Password);

        // 3. Map DTO to the Domain Entity (Keeping your UserName!)
        var newUser = new User
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
            UserName = dto.Email,
            PasswordHash = hashedPassword
        };

        // 4. Save to Database and Assign Role (Keeping your Role logic!)
        await _userRepository.AddAsync(newUser, cancellationToken);
        await _userRepository.AssignRoleAsync(newUser, Roles.Student.ToString(), cancellationToken);

        // 5. Generate the 6-digit OTP
        string otp = new Random().Next(100000, 999999).ToString();

        // 6. Save the OTP to the AspNetUserTokens table natively
        var tokenResult = await _userRepository.SaveVerificationTokenAsync(newUser, "CareerPathApp", "EmailConfirmationOTP", otp, cancellationToken);

        if (!tokenResult.IsSuccess)
        {
            return Result.Failure("Account created, but failed to generate verification token.");
        }

        // 7. Send the Email via Mailtrap
        string emailBody = $@"
        <h2>Welcome to CareerPath!</h2>
        <p>Your email verification code is: <strong>{otp}</strong></p>
        <p>Please enter this code in the app to activate your account.</p>";

        await _emailService.SendEmailAsync(newUser.Email, "Verify Your CareerPath Account", emailBody);

        return Result.Success();
    }
}