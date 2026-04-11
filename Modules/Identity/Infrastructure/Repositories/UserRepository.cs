using CareerPath.Identity.Core.Contracts;
using CareerPath.Identity.Core.Entities;
using Microsoft.AspNetCore.Identity;



namespace CareerPath.Identity.Infrastructure.Repositories;

public class UserRepository(UserManager<User> userManager) : IUserRepository
{
    private readonly UserManager<User> _userManager = userManager;

    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _userManager.FindByEmailAsync(email);
    }

    public async Task<bool> IsEmailUniqueAsync(string email, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByEmailAsync(email);
        return user == null;
    }

    public async Task AddAsync(User user, CancellationToken cancellationToken = default)
    {
        // Since our Core handler already hashed the password and mapped it to user.PasswordHash,
        // we use the CreateAsync method that does not take a separate password parameter.
        var result = await _userManager.CreateAsync(user);

        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            throw new InvalidOperationException($"User creation failed: {errors}");
        }
    }

    public async Task AssignRoleAsync(User user, string role, CancellationToken cancellationToken = default)
    {
        var result = await _userManager.AddToRoleAsync(user, role);

        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            throw new InvalidOperationException($"Failed to assign role: {errors}");
        }
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // UserManager automatically calls SaveChanges to the database internally 
        // when CreateAsync and AddToRoleAsync are called. 
        // Therefore, this method can safely do nothing in this specific implementation.
        return Task.CompletedTask;
    }
    public async Task SaveVerificationTokenAsync(User user, string provider, string tokenName, string token, CancellationToken cancellationToken = default)
    {
        // This natively saves the token to the AspNetUserTokens table linked to this user
        var identityResult = await _userManager.SetAuthenticationTokenAsync(user, provider, tokenName, token);

        if (!identityResult.Succeeded)
        {
            var errors = string.Join(", ", identityResult.Errors.Select(e => e.Description));
            throw new InvalidOperationException($"Failed to save verification token to the database: {errors}");
        }
    }
    public async Task<bool> VerifyOtpAsync(User user, string provider, string tokenName, string providedOtp, CancellationToken cancellationToken = default)
    {
        // Fetch the token natively from the AspNetUserTokens table
        string? storedToken = await _userManager.GetAuthenticationTokenAsync(user, provider, tokenName);

        if (storedToken == providedOtp)
        {
            // If it matches, delete it so it cannot be used again (One-Time Password)
            await _userManager.RemoveAuthenticationTokenAsync(user, provider, tokenName);
            return true;
        }

        return false;
    }

    public async Task ConfirmEmailAsync(User user, CancellationToken cancellationToken = default)
    {
        user.EmailConfirmed = true;
        var result = await _userManager.UpdateAsync(user);

        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            throw new InvalidOperationException($"Failed to update user email confirmation status: {errors}");
        }
    }
    public async Task<string?> GetRoleAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());

        if (user == null)
        {
            return null;
        }

        // 3. Ask the framework for the user's roles
        var roles = await _userManager.GetRolesAsync(user);

        // 4. Return the first role, or null if the list is empty
        return roles.FirstOrDefault();
    }
}