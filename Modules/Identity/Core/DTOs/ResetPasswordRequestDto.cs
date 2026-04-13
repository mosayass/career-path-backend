namespace CareerPath.Identity.Core.DTOs;

public record ResetPasswordRequestDto(
    string Email,
    string Token,
    string NewPassword);