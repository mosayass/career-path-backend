using System;
using System.Threading;
using System.Threading.Tasks;
using CareerPath.Identity.Core.Entities;
using CareerPath.Shared.Responses;
namespace CareerPath.Identity.Core.Contracts;

public interface IUserRepository
{
    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<bool> IsEmailUniqueAsync(string email, CancellationToken cancellationToken = default);
    Task AddAsync(User user, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
    Task AssignRoleAsync(User user, string role, CancellationToken cancellationToken = default);
    Task SaveVerificationTokenAsync(User user, string provider, string tokenName, string token, CancellationToken cancellationToken = default);
    Task<bool> VerifyOtpAsync(User user, string provider, string tokenName, string providedOtp, CancellationToken cancellationToken = default);
    Task ConfirmEmailAsync(User user, CancellationToken cancellationToken = default);
}