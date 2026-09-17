using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using AbpGoat.Permissions;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Content;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Users;

namespace AbpGoat.Vulnerable.Documents;

/// <summary>
/// Document management. Deliberately insecure — see VULNERABILITIES.md (VL-003, VL-004, VL-009).
/// The class-level [Authorize] only proves the caller is authenticated; the per-operation
/// authorization and ownership checks are where the intentional defects live.
/// </summary>
[Authorize(AbpGoatPermissions.Documents.Default)]
public class DocumentAppService : ApplicationService, IDocumentAppService
{
    private readonly IRepository<Document, Guid> _repository;
    private readonly DocumentStorageOptions _storageOptions;

    public DocumentAppService(
        IRepository<Document, Guid> repository,
        IOptions<DocumentStorageOptions> storageOptions)
    {
        _repository = repository;
        _storageOptions = storageOptions.Value;
    }

    [Authorize(AbpGoatPermissions.Documents.Create)]
    public async Task<DocumentDto> CreateAsync(CreateDocumentDto input)
    {
        Directory.CreateDirectory(_storageOptions.BasePath);

        // The stored name is derived from the id, so uploads themselves are safe;
        // the traversal defect is only on the download path (VL-004).
        var storedName = GuidGenerator.Create().ToString("N") + Path.GetExtension(input.FileName);
        var fullPath = Path.Combine(_storageOptions.BasePath, storedName);
        await File.WriteAllBytesAsync(fullPath, input.Content);

        var document = new Document(
            GuidGenerator.Create(),
            input.Title,
            storedName,
            CurrentUser.GetId(),
            CurrentTenant.Id);

        await _repository.InsertAsync(document);

        return ObjectMapper.Map<Document, DocumentDto>(document);
    }

    // VL-003 (CWE-639, IDOR): loads any document by id with no ownership check.
    // Authentication is enforced by the class-level [Authorize], but authorization is not:
    // any authenticated user can read another user's document.
    public async Task<DocumentDto> GetAsync(Guid id)
    {
        var document = await _repository.GetAsync(id);
        return ObjectMapper.Map<Document, DocumentDto>(document);
    }

    public async Task<PagedResultDto<DocumentDto>> GetListAsync(PagedAndSortedResultRequestDto input)
    {
        // Safe baseline: a user only lists their own documents.
        var ownerId = CurrentUser.GetId();
        var queryable = await _repository.GetQueryableAsync();

        var query = queryable
            .Where(d => d.OwnerUserId == ownerId)
            .OrderBy(input.Sorting.IsNullOrWhiteSpace() ? nameof(Document.Title) : input.Sorting)
            .Skip(input.SkipCount)
            .Take(input.MaxResultCount);

        var documents = await AsyncExecuter.ToListAsync(query);
        var totalCount = await AsyncExecuter.CountAsync(
            queryable.Where(d => d.OwnerUserId == ownerId));

        return new PagedResultDto<DocumentDto>(
            totalCount,
            ObjectMapper.Map<List<Document>, List<DocumentDto>>(documents));
    }

    // VL-004 (CWE-22, path traversal): the caller-supplied file name is joined onto the
    // storage base path with no normalisation or containment check, so a name like
    // "../appsettings.json" escapes the blob root and reads arbitrary files. Chains into
    // VL-006 (hardcoded secrets in appsettings.json).
    public async Task<IRemoteStreamContent> DownloadAsync(string fileName)
    {
        var fullPath = Path.Combine(_storageOptions.BasePath, fileName);
        var bytes = await File.ReadAllBytesAsync(fullPath);

        return new RemoteStreamContent(
            new MemoryStream(bytes),
            fileName,
            "application/octet-stream");
    }

    // VL-009 (CWE-862): a "Documents.Delete" permission is defined and shown in the UI,
    // but this method neither carries [Authorize(AbpGoatPermissions.Documents.Delete)]
    // nor calls CheckAsync — so the Default (view) permission is enough to delete.
    public async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }
}
