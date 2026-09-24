using System;

namespace AbpGoat.Vulnerable.Events;

[Serializable]
public class TenantReportEto
{
    public Guid? TenantId { get; set; }
}
