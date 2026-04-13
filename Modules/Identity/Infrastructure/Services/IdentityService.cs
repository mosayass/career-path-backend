using CareerPath.Identity.Core.Contracts;
using CareerPath.Identity.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using System.Threading.Tasks;

namespace CareerPath.Identity.Infrastructure.Services;

public class IdentityService(UserManager<User> userManager) : IIdentityService
{
    public async Task<string> GeneratePasswordResetTokenAsync(User user)
    {
        var token = await userManager.GeneratePasswordResetTokenAsync(user);
        return WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
    }

    public async Task<(bool Succeeded, string? ErrorMessage)> ResetPasswordAsync(User user, string token, string newPassword)
    {
        // 1. Decode the token exactly like you did before
        var decodedTokenBytes = WebEncoders.Base64UrlDecode(token);
        var originalToken = Encoding.UTF8.GetString(decodedTokenBytes);

        // 2. Call the native method (This automatically verifies the token, uses your BCrypt hasher, updates the stamp, and saves!)
        var result = await userManager.ResetPasswordAsync(user, originalToken, newPassword);

        return result.Succeeded
            ? (true, null)
            : (false, result.Errors.FirstOrDefault()?.Description);
    }
    public async Task<(bool Succeeded, string? ErrorMessage)> RegisterUserAsync(User user, string password, string role)
    {
        // 1. Microsoft hashes the password with BCrypt and saves the user
        var result = await userManager.CreateAsync(user, password);
        if (!result.Succeeded) return (false, result.Errors.FirstOrDefault()?.Description);

        // 2. Microsoft assigns the role safely
        var roleResult = await userManager.AddToRoleAsync(user, role);
        if (!roleResult.Succeeded) return (false, roleResult.Errors.FirstOrDefault()?.Description);

        return (true, null);
    }
    public async Task<bool> CheckPasswordAsync(User user, string password)
    {
        // Microsoft takes the user and the plain-text password, 
        // runs it through your injected BCrypt class behind the scenes, 
        // and returns true or false.
        return await userManager.CheckPasswordAsync(user, password);
    }
}