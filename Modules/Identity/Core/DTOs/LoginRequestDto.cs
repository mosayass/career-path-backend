namespace CareerPath.Identity.Core.DTOs;

public record LoginRequestDto(
    string Email,
    string Password
);