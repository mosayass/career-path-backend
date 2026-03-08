using System;

namespace CareerPath.Identity.Core.DTOs;

public record RegisterRequestDto(
    string FirstName,
    string LastName,
    string Email,
    string Password
);