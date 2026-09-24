using Riok.Mapperly.Abstractions;
using Volo.Abp.Mapperly;

namespace AbpGoat.Orders;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class AbpGoatOrderToOrderDtoMapper : MapperBase<Order, OrderDto>
{
    public override partial OrderDto Map(Order source);

    public override partial void Map(Order source, OrderDto destination);
}
