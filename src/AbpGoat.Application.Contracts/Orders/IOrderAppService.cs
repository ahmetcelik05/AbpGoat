using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace AbpGoat.Orders;

public interface IOrderAppService : IApplicationService
{
    Task<OrderDto> PlaceOrderAsync(PlaceOrderDto input);

    Task<OrderDto> GetAsync(Guid id);

    Task MarkPaidAsync(Guid id);
}
