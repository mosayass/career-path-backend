using Microsoft.AspNetCore.Identity;

namespace CareerPath.Identity.Core.Entities;

public class User : IdentityUser<Guid>
{
    // Add custom domain properties here
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public bool IsActive { get; set; } = true;
}