using CareerPath.Identity.Core.DTOs;
using MediatR;
using CareerPath.Identity.Core.Contracts;
using CareerPath.Identity.Core.Entities;

namespace CareerPath.Identity.Core.Features.Commands.Register;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, RegisterResponseDto>
{
    private readonly IUserRepository _userRepository;

    private readonly IPasswordHasher _passwordHasher;

    public RegisterCommandHandler(IUserRepository userRepository, IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository ;
        _passwordHasher = passwordHasher;
    }

    public async Task<RegisterResponseDto> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var dto = request.RequestDto;

        //1.Check if user already exists in the database
         var existingUser = await _userRepository.GetByEmailAsync(dto.Email, cancellationToken);
        if (existingUser != null)
            return new RegisterResponseDto(Guid.Empty, false, "Email already in use.");

        //2.Hash the password securely
        var hashedPassword = _passwordHasher.Hash(dto.Password);

        //3.Map DTO to the Domain Entity(User)
        var newUser = new User
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
            UserName = dto.Email, // IdentityUser usually requires UserName to be set
            PasswordHash = hashedPassword
        };

        //4.Save to PostgreSQL Database
        await _userRepository.AddAsync(newUser, cancellationToken);

        //5.Return the successful response
         return new RegisterResponseDto(newUser.Id, true, "Registration successful.");
    }
}