using CareerPath.Identity.Core.Entities; // Assuming User entity is here
using System.Threading.Tasks;

namespace CareerPath.Identity.Core.Contracts;

public interface IIdentityService
{
    Task<string> GeneratePasswordResetTokenAsync(User user);

    Task<(bool Succeeded, string? ErrorMessage)>
        ResetPasswordAsync(
        User user,
        string token,
        string newPassword);
    Task<(bool Succeeded, string? ErrorMessage)>
        RegisterUserAsync(
        User user,
        string password,
        string role);
    Task<bool> CheckPasswordAsync(
        User user,
        string password);
}