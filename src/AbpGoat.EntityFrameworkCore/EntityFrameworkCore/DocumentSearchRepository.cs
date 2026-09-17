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
/// Custom document repository. Deliberately insecure — see VULNERABILITIES.md (VL-001).
/// </summary>
public class DocumentSearchRepository
    : EfCoreRepository<AbpGoatDbContext, Document, Guid>, IDocumentSearchRepository
{
    public DocumentSearchRepository(IDbContextProvider<AbpGoatDbContext> dbContextProvider)
        : base(dbContextProvider)
    {
    }

    // VL-001 (CWE-89, SQL injection): the search term is concatenated straight into raw SQL,
    // so input such as "%'; DROP TABLE ... --" is executed by the database. A parameterised
    // query (FromSqlInterpolated) or a LINQ Where would be safe.
    public async Task<List<Document>> SearchByTitleAsync(string term)
    {
        var dbSet = await GetDbSetAsync();
        var sql = "SELECT * FROM \"AppDocuments\" WHERE \"Title\" LIKE '%" + term + "%'";
        return await dbSet.FromSqlRaw(sql).ToListAsync();
    }
}
