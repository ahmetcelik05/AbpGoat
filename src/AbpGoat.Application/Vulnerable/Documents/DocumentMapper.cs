using Riok.Mapperly.Abstractions;
using Volo.Abp.Mapperly;

namespace AbpGoat.Vulnerable.Documents;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class AbpGoatDocumentToDocumentDtoMapper : MapperBase<Document, DocumentDto>
{
    public override partial DocumentDto Map(Document source);

    public override partial void Map(Document source, DocumentDto destination);
}
