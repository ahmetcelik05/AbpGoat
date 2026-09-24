using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AbpGoat.Vulnerable.Documents;
using AbpGoat.Vulnerable.Reports;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace AbpGoat.EntityFrameworkCore;

/// <summary>
/// Custom document repository providing title search.
/// </summary>
public class DocumentSearchRepository
    : EfCoreRepository<AbpGoatDbContext, Document, Guid>, IDocumentSearchRepository
{
    public DocumentSearchRepository(IDbContextProvider<AbpGoatDbContext> dbContextProvider)
        : base(dbContextProvider)
    {
    }

    public async Task<List<Document>> SearchByTitleAsync(string term)
    {
        var dbSet = await GetDbSetAsync();
        var sql = "SELECT * FROM \"AppDocuments\" WHERE \"Title\" LIKE '%" + term + "%'";
        return await dbSet.FromSqlRaw(sql).ToListAsync();
    }

    public async Task<List<Document>> SearchByTitleSafeAsync(string term)
    {
        var dbSet = await GetDbSetAsync();
        var pattern = "%" + term + "%";
        return await dbSet
            .FromSqlInterpolated($"SELECT * FROM \"AppDocuments\" WHERE \"Title\" LIKE {pattern}")
            .ToListAsync();
    }
}
