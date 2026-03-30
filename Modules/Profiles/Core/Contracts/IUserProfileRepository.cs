using System;
using System.Threading;
using System.Threading.Tasks;
using CareerPath.Profiles.Core.Entities;

namespace CareerPath.Profiles.Core.Contracts
{
    public interface IUserProfileRepository
    {
        Task<UserProfile?> GetByIdAsync(Guid userId, CancellationToken cancellationToken = default);

        Task AddAsync(UserProfile profile, CancellationToken cancellationToken = default);

        Task<bool> ExistsAsync(Guid userId, CancellationToken cancellationToken = default);
    }
}