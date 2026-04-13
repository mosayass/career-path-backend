using BCrypt.Net;
using CareerPath.Identity.Core.Contracts;
using CareerPath.Identity.Core.Entities;
using Microsoft.AspNetCore.Identity;

namespace CareerPath.Identity.Infrastructure.Security;

public class PasswordHasher : Microsoft.AspNetCore.Identity.IPasswordHasher<User>
{
    public string HashPassword(User user, string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    public PasswordVerificationResult VerifyHashedPassword(User user, string hashedPassword, string providedPassword)
    {
        var isValid = BCrypt.Net.BCrypt.Verify(providedPassword, hashedPassword);
        return isValid ? PasswordVerificationResult.Success : PasswordVerificationResult.Failed;
    }
}