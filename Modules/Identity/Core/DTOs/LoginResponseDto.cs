namespace CareerPath.Identity.Core.DTOs;

public record LoginResponseDto(
    string Token,
    string Email,
    string FullName
);