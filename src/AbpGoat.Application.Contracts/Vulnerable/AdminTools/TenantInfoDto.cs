using System;

namespace AbpGoat.Vulnerable.AdminTools;

public class TenantInfoDto
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;
}
