using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Content;

namespace AbpGoat.Vulnerable.Documents;

public interface IDocumentAppService : IApplicationService
{
    Task<DocumentDto> CreateAsync(CreateDocumentDto input);

    Task<DocumentDto> GetAsync(Guid id);

    Task<PagedResultDto<DocumentDto>> GetListAsync(PagedAndSortedResultRequestDto input);

    Task<IRemoteStreamContent> DownloadAsync(string fileName);

    Task DeleteAsync(Guid id);

    Task<List<string>> GetMyDocumentTitlesAsync();
}
