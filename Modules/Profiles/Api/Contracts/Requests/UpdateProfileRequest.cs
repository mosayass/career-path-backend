using System;
using System.Collections.Generic;
using System.Text;

namespace CareerPath.Profiles.Api.Contracts.Requests
{
    public record UpdateProfileRequest(string DisplayName, string? Bio, string? AvatarUrl);
}
