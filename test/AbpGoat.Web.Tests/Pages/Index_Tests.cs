using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace AbpGoat.Pages;

[Collection(AbpGoatTestConsts.CollectionDefinitionName)]
public class Index_Tests : AbpGoatWebTestBase
{
    [Fact]
    public async Task Welcome_Page()
    {
        var response = await GetResponseAsStringAsync("/");
        response.ShouldNotBeNull();
    }
}
