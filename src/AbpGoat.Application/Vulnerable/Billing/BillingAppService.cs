using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Services;

namespace AbpGoat.Vulnerable.Billing;

[Authorize]
public class BillingAppService : ApplicationService, IBillingAppService
{
    public Task SavePaymentAsync(SavePaymentDto input)
    {
        // Persisting the payment is out of scope for the sample. The audit-logging concern is
        // that the input DTO (with CardNumber/Cvv) is recorded as the action's parameters.
        return Task.CompletedTask;
    }
}
