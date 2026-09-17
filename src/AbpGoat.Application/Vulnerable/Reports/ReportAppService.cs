using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using AbpGoat.Permissions;
using AbpGoat.Vulnerable.Documents;
using Volo.Abp.Application.Services;
using Volo.Abp.Data;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.MultiTenancy;

namespace AbpGoat.Vulnerable.Reports;

/// <summary>
/// Reporting over documents. Deliberately insecure — see VULNERABILITIES.md (VL-001, VL-008).
/// </summary>
[Authorize(AbpGoatPermissions.Documents.Default)]
public class ReportAppService : ApplicationService, IReportAppService
{
    private readonly IDocumentSearchRepository _searchRepository;
    private readonly IRepository<Document, System.Guid> _documentRepository;
    private readonly IDataFilter _dataFilter;

    public ReportAppService(
        IDocumentSearchRepository searchRepository,
        IRepository<Document, System.Guid> documentRepository,
        IDataFilter dataFilter)
    {
        _searchRepository = searchRepository;
        _documentRepository = documentRepository;
        _dataFilter = dataFilter;
    }

    // VL-001: delegates to the raw-SQL repository method (SQL injection lives there).
    public async Task<List<ReportItemDto>> SearchAsync(string term)
    {
        var documents = await _searchRepository.SearchByTitleAsync(term);
        return documents.Select(MapToReportItem).ToList();
    }

    // VL-008 (CWE-639): disabling the IMultiTenant data filter returns documents from every
    // tenant to a single-tenant caller, breaking tenant isolation.
    public async Task<List<ReportItemDto>> GetCrossTenantAsync()
    {
        using (_dataFilter.Disable<IMultiTenant>())
        {
            var documents = await _documentRepository.GetListAsync();
            return documents.Select(MapToReportItem).ToList();
        }
    }

    private static ReportItemDto MapToReportItem(Document document)
    {
        return new ReportItemDto
        {
            Id = document.Id,
            Title = document.Title,
            OwnerUserId = document.OwnerUserId,
            TenantId = document.TenantId
        };
    }
}
