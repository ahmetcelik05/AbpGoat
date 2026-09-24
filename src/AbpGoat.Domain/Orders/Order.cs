using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace AbpGoat.Orders;

public class Order : FullAuditedAggregateRoot<Guid>
{
    public Guid CustomerUserId { get; set; }

    public string ProductName { get; set; } = string.Empty;

    public int Quantity { get; set; }

    public decimal UnitPrice { get; set; }

    public decimal TotalPrice { get; set; }

    public OrderStatus Status { get; set; }
}
