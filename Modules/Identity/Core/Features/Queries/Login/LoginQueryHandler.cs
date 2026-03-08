using CareerPath.Identity.Core.DTOs;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace CareerPath.Identity.Core.Features.Queries.Login;

public class LoginQueryHandler : IRequestHandler<LoginQuery, LoginResponseDto>
{
    // Dependencies like IUserRepository, IPasswordHasher, and IJwtProvider will be injected here later

    public LoginQueryHandler()
    {
    }

    public async Task<LoginResponseDto> Handle(LoginQuery request, CancellationToken cancellationToken)
    {
        var dto = request.RequestDto;

        // 1. Fetch the user by email
        // var user = await _userRepository.GetByEmailAsync(dto.Email, cancellationToken);
        // if (user == null)
        //     throw new UnauthorizedAccessException("Invalid credentials.");

        // 2. Verify the password
        // var isPasswordValid = _passwordHasher.Verify(user.PasswordHash, dto.Password);
        // if (!isPasswordValid)
        //     throw new UnauthorizedAccessException("Invalid credentials.");

        // 3. Generate JWT (This will be implemented in Phase 3)
        // var token = _jwtProvider.GenerateToken(user);

        // 4. Return the successful response
        // return new LoginResponseDto(token, user.Email, $"{user.FirstName} {user.LastName}");

        // Temporary return to satisfy the compiler until dependencies are wired up
        return await Task.FromResult(new LoginResponseDto("dummy-jwt-token", dto.Email, "Dummy Name"));
    }
}