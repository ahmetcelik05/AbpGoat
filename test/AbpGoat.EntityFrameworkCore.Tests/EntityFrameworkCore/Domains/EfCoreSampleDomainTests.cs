using AbpGoat.Samples;
using Xunit;

namespace AbpGoat.EntityFrameworkCore.Domains;

[Collection(AbpGoatTestConsts.CollectionDefinitionName)]
public class EfCoreSampleDomainTests : SampleDomainTests<AbpGoatEntityFrameworkCoreTestModule>
{

}
