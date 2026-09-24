using System;
using Volo.Abp.Application.Dtos;

namespace AbpGoat.Orders;

public class OrderDto : AuditedEntityDto<Guid>
{
    public Guid CustomerUserId { get; set; }

    public string ProductName { get; set; } = string.Empty;

    public int Quantity { get; set; }

    public decimal UnitPrice { get; set; }

    public decimal TotalPrice { get; set; }

    public OrderStatus Status { get; set; }
}
