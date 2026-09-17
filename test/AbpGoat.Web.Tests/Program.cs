using Microsoft.AspNetCore.Builder;
using AbpGoat;
using Volo.Abp.AspNetCore.TestBase;

var builder = WebApplication.CreateBuilder();
builder.Environment.ContentRootPath = GetWebProjectContentRootPathHelper.Get("AbpGoat.Web.csproj"); 
await builder.RunAbpModuleAsync<AbpGoatWebTestModule>(applicationName: "AbpGoat.Web");

public partial class Program
{
}
