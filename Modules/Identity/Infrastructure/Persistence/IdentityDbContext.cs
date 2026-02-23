using CareerPath.Identity.Core.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace CareerPath.Identity.Infrastructure.Persistence;

public class IdentityDbContext : IdentityDbContext<User, Role, Guid>
{
    public IdentityDbContext(DbContextOptions<IdentityDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Enforce the modular schema isolation
        const string schema = "identity";
        builder.HasDefaultSchema(schema);

        // Rename default Identity tables to remove "AspNet" prefixes
        builder.Entity<User>(entity =>
        {
            entity.ToTable("Users", schema);
        });

        builder.Entity<Role>(entity =>
        {
            entity.ToTable("Roles", schema);
        });

        builder.Entity<Microsoft.AspNetCore.Identity.IdentityUserRole<Guid>>(entity =>
        {
            entity.ToTable("UserRoles", schema);
        });

        builder.Entity<Microsoft.AspNetCore.Identity.IdentityUserClaim<Guid>>(entity =>
        {
            entity.ToTable("UserClaims", schema);
        });

        builder.Entity<Microsoft.AspNetCore.Identity.IdentityUserLogin<Guid>>(entity =>
        {
            entity.ToTable("UserLogins", schema);
        });

        builder.Entity<Microsoft.AspNetCore.Identity.IdentityRoleClaim<Guid>>(entity =>
        {
            entity.ToTable("RoleClaims", schema);
        });

        builder.Entity<Microsoft.AspNetCore.Identity.IdentityUserToken<Guid>>(entity =>
        {
            entity.ToTable("UserTokens", schema);
        });
    }
}