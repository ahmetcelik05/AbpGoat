using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace AbpGoat.Vulnerable.Billing;

public interface IBillingAppService : IApplicationService
{
    Task SavePaymentAsync(SavePaymentDto input);
}
