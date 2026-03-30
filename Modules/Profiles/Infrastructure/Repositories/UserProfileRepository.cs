using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CareerPath.Profiles.Core.Contracts;
using CareerPath.Profiles.Core.Entities;
using CareerPath.Profiles.Infrastructure.Persistence;

namespace CareerPath.Profiles.Infrastructure.Repositories
{
    public class UserProfileRepository : IUserProfileRepository
    {
        private readonly ProfilesDbContext _context;

        public UserProfileRepository(ProfilesDbContext context)
        {
            _context = context;
        }

        public async Task<UserProfile?> GetByIdAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            return await _context.UserProfiles
                .FirstOrDefaultAsync(p => p.UserId == userId, cancellationToken);
        }

        public async Task AddAsync(UserProfile profile, CancellationToken cancellationToken = default)
        {
            await _context.UserProfiles.AddAsync(profile, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<bool> ExistsAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            return await _context.UserProfiles
                .AnyAsync(p => p.UserId == userId, cancellationToken);
        }
    }
}