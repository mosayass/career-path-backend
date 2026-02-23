using Microsoft.AspNetCore.Identity;

namespace CareerPath.Identity.Core.Entities;

public class Role : IdentityRole<Guid>
{
    public string? Description { get; set; }

    public Role() : base() { }

    public Role(string roleName) : base(roleName) { }
}