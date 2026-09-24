using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace AbpGoat.Vulnerable.Approvals;

public interface IDocumentApprovalAppService : IApplicationService
{
    Task ApproveAllAsync();
}
