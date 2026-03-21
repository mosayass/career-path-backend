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
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtProvider _jwtProvider;

    public LoginQueryHandler(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IJwtProvider jwtProvider)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _jwtProvider = jwtProvider;
    }

    public async Task<Result<LoginResponseDto>> Handle(LoginQuery request, CancellationToken cancellationToken)
    {
        // 1. Find the user by email
        var user = await _userRepository.GetByEmailAsync(request.RequestDto.Email, cancellationToken);

        if (user is null)
        {
            // We return a generic message for security so hackers don't know which emails exist
            return Result<LoginResponseDto>.Failure("Invalid email or password.");
        }

        // 2. Verify the password
        bool isPasswordValid = _passwordHasher.Verify(request.RequestDto.Password, user.PasswordHash);

        if (!isPasswordValid)
        {
            return Result<LoginResponseDto>.Failure("Invalid email or password.");
        }

        // 3. Generate the JWT Token
        string token = _jwtProvider.GenerateToken(user);

        // 4. Return success with the DTO
        var response = new LoginResponseDto(token, user.Email, $"{user.FirstName} {user.LastName}");

        return Result<LoginResponseDto>.Success(response);
    }
}