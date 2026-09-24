using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AbpGoat.Vulnerable.Documents;
using Volo.Abp.Domain.Repositories;

namespace AbpGoat.Vulnerable.Reports;

public interface IDocumentSearchRepository : IRepository<Document, Guid>
{
    Task<List<Document>> SearchByTitleAsync(string term);

    Task<List<Document>> SearchByTitleSafeAsync(string term);
}
