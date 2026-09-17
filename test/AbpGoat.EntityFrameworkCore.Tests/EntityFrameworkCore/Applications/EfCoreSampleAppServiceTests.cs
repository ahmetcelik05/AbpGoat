using AbpGoat.Samples;
using Xunit;

namespace AbpGoat.EntityFrameworkCore.Applications;

[Collection(AbpGoatTestConsts.CollectionDefinitionName)]
public class EfCoreSampleAppServiceTests : SampleAppServiceTests<AbpGoatEntityFrameworkCoreTestModule>
{

}
