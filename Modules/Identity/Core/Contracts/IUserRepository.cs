using System;
using System.Threading;
using System.Threading.Tasks;
using CareerPath.Identity.Core.Entities; 

namespace CareerPath.Identity.Core.Contracts;

public interface IUserRepository
{
    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<bool> IsEmailUniqueAsync(string email, CancellationToken cancellationToken = default);
    Task AddAsync(User user, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}