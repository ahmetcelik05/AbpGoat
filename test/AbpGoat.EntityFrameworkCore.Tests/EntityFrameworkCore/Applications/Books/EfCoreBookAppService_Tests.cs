using AbpGoat.Books;
using Xunit;

namespace AbpGoat.EntityFrameworkCore.Applications.Books;

[Collection(AbpGoatTestConsts.CollectionDefinitionName)]
public class EfCoreBookAppService_Tests : BookAppService_Tests<AbpGoatEntityFrameworkCoreTestModule>
{

}