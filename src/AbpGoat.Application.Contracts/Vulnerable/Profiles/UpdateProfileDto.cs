using System.Collections.Generic;

namespace AbpGoat.Vulnerable.Profiles;

public class UpdateProfileDto
{
    public string? Name { get; set; }

    public string? Surname { get; set; }

    public List<string> RoleNames { get; set; } = new();

    public Dictionary<string, object> ExtraProperties { get; set; } = new();
}
