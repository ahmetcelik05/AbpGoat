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
/// Document management application service.
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

        // The stored name is derived from a generated id plus the original extension.
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

    public async Task<DocumentDto> GetAsync(Guid id)
    {
        var document = await _repository.GetAsync(id);
        return ObjectMapper.Map<Document, DocumentDto>(document);
    }

    public async Task<PagedResultDto<DocumentDto>> GetListAsync(PagedAndSortedResultRequestDto input)
    {
        // A user lists their own documents.
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

    public async Task<IRemoteStreamContent> DownloadAsync(string fileName)
    {
        var fullPath = Path.Combine(_storageOptions.BasePath, fileName);
        var bytes = await File.ReadAllBytesAsync(fullPath);

        return new RemoteStreamContent(
            new MemoryStream(bytes),
            fileName,
            "application/octet-stream");
    }

    public async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public async Task<List<string>> GetMyDocumentTitlesAsync()
    {
        var ownerId = CurrentUser.GetId();
        var queryable = await _repository.GetQueryableAsync();

        return await AsyncExecuter.ToListAsync(
            queryable
                .Where(d => d.OwnerUserId == ownerId)
                .OrderBy(d => d.Title)
                .Select(d => d.Title));
    }
}
