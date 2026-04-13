using CareerPath.Identity.Core.Contracts;
using CareerPath.Identity.Core.DTOs;
using CareerPath.Shared.Responses;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace CareerPath.Identity.Core.Features.Queries.Login;

public class LoginQueryHandler : IRequestHandler<LoginQuery, Result<LoginResponseDto>>
{
    private readonly IUserRepository _userRepository;
    private readonly IIdentityService _identityService;
    private readonly IJwtProvider _jwtProvider;

    public LoginQueryHandler(
        IUserRepository userRepository,
        IIdentityService identityService,
        IJwtProvider jwtProvider)
    {
        _userRepository = userRepository;
        _identityService = identityService;
        _jwtProvider = jwtProvider;
    }

    public async Task<Result<LoginResponseDto>> Handle(LoginQuery request, CancellationToken cancellationToken)
    {
        // 1. Find the user by email
        var user = await _userRepository.GetByEmailAsync(request.RequestDto.Email, cancellationToken);

        if (user is null)
        {
            // We return a generic message for security so hackers don't know which emails exist
            return Result<LoginResponseDto>.Failure(ErrorType.Unauthorized, "Invalid email or password.");
        }

        // 2. Verify the password
        bool isPasswordValid = await _identityService.CheckPasswordAsync(user, request.RequestDto.Password);
        if (!isPasswordValid)
        {
            return Result<LoginResponseDto>.Failure(ErrorType.Unauthorized, "Invalid email or password.");
        }

        // 3. Check if the email is confirmed
        if (!user.EmailConfirmed)
        {
            return Result<LoginResponseDto>.Failure(ErrorType.Forbidden, "Email not confirmed. Please check your inbox.");
        }

        // 4. Check if the account is active (Soft Delete support)
        if (!user.IsActive)
        {
            return Result<LoginResponseDto>.Failure(ErrorType.Forbidden, "Account is deactivated. Please contact support.");
        }

        // 5. Generate the JWT Token
        string token = _jwtProvider.GenerateToken(user);

        // 6. Return success with the DTO
        var response = new LoginResponseDto(token, user.Email, $"{user.FirstName} {user.LastName}");

        return Result<LoginResponseDto>.Success(response);
    }
}