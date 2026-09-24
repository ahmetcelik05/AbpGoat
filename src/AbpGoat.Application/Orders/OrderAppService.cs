using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Users;

namespace AbpGoat.Orders;

[Authorize]
public class OrderAppService : ApplicationService, IOrderAppService
{
    private readonly IRepository<Order, Guid> _repository;

    public OrderAppService(IRepository<Order, Guid> repository)
    {
        _repository = repository;
    }

    public async Task<OrderDto> PlaceOrderAsync(PlaceOrderDto input)
    {
        var order = new Order
        {
            CustomerUserId = CurrentUser.GetId(),
            ProductName = input.ProductName,
            Quantity = input.Quantity,
            UnitPrice = input.UnitPrice,
            TotalPrice = input.Quantity * input.UnitPrice,
            Status = OrderStatus.Pending
        };

        await _repository.InsertAsync(order, autoSave: true);

        return ObjectMapper.Map<Order, OrderDto>(order);
    }

    public async Task<OrderDto> GetAsync(Guid id)
    {
        var order = await _repository.GetAsync(id);
        return ObjectMapper.Map<Order, OrderDto>(order);
    }

    public async Task MarkPaidAsync(Guid id)
    {
        var order = await _repository.GetAsync(id);
        order.Status = OrderStatus.Paid;
        await _repository.UpdateAsync(order, autoSave: true);
    }
}
