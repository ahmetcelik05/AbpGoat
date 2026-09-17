using System;

namespace AbpGoat.Vulnerable.Reports;

public class ReportItemDto
{
    public Guid Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public Guid OwnerUserId { get; set; }

    public Guid? TenantId { get; set; }
}
