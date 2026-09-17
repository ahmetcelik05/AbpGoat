using System.Collections.Generic;

namespace AbpGoat.Vulnerable.Profiles;

public class UpdateProfileDto
{
    public string? Name { get; set; }

    public string? Surname { get; set; }

    /// <summary>Role names applied to the current user (intentionally attacker-controllable).</summary>
    public List<string> RoleNames { get; set; } = new();
}
