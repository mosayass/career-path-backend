namespace CareerPath.Identity.Core.DTOs;

public record RegisterResponseDto(
    Guid UserId,
    bool IsSuccess,
    string Message
);