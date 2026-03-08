using CareerPath.Identity.Core.Entities;

namespace CareerPath.Identity.Core.Contracts;

public interface IJwtProvider
{
    string GenerateToken(User user);
}