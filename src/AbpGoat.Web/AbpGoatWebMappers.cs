using Riok.Mapperly.Abstractions;
using Volo.Abp.Mapperly;
using AbpGoat.Authors;
using AbpGoat.Books;
namespace AbpGoat.Web;
[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class AbpGoatWebMappers : MapperBase<BookDto, CreateUpdateBookDto>
{
    public override partial CreateUpdateBookDto Map(BookDto source);
    public override partial void Map(BookDto source, CreateUpdateBookDto destination);
}
[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class AbpGoatAuthorDtoToCreateUpdateAuthorDtoMapper : MapperBase<AuthorDto, CreateUpdateAuthorDto>
{
    public override partial CreateUpdateAuthorDto Map(AuthorDto source);
    public override partial void Map(AuthorDto source, CreateUpdateAuthorDto destination);
}
