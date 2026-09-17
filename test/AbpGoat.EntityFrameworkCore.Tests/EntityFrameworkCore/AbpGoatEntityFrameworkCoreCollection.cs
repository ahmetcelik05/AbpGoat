using Xunit;

namespace AbpGoat.EntityFrameworkCore;

[CollectionDefinition(AbpGoatTestConsts.CollectionDefinitionName)]
public class AbpGoatEntityFrameworkCoreCollection : ICollectionFixture<AbpGoatEntityFrameworkCoreFixture>
{

}
