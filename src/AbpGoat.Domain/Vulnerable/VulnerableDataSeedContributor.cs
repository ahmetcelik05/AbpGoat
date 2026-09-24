using System;
using System.Linq;
using System.Threading.Tasks;
using AbpGoat.Vulnerable.Documents;
using Volo.Abp;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;
using Volo.Abp.Identity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.TenantManagement;

namespace AbpGoat.Vulnerable;

/// <summary>
/// Seeds tenants, non-admin users and documents so the app runs against realistic,
/// multi-owner data.
/// </summary>
public class VulnerableDataSeedContributor : IDataSeedContributor, ITransientDependency
{
    private const string DefaultPassword = "1q2w3E*";

    private readonly IdentityUserManager _userManager;
    private readonly IRepository<Document, Guid> _documentRepository;
    private readonly ITenantManager _tenantManager;
    private readonly ITenantRepository _tenantRepository;
    private readonly ICurrentTenant _currentTenant;
    private readonly IGuidGenerator _guidGenerator;

    public VulnerableDataSeedContributor(
        IdentityUserManager userManager,
        IRepository<Document, Guid> documentRepository,
        ITenantManager tenantManager,
        ITenantRepository tenantRepository,
        ICurrentTenant currentTenant,
        IGuidGenerator guidGenerator)
    {
        _userManager = userManager;
        _documentRepository = documentRepository;
        _tenantManager = tenantManager;
        _tenantRepository = tenantRepository;
        _currentTenant = currentTenant;
        _guidGenerator = guidGenerator;
    }

    public async Task SeedAsync(DataSeedContext context)
    {
        // Only orchestrate from the host pass; seed the host and every tenant from here so the
        // data set is self-contained regardless of the migrator's per-tenant loop.
        if (context.TenantId != null)
        {
            return;
        }

        await SeedContextAsync(null);

        if (MultiTenancy.MultiTenancyConsts.IsEnabled)
        {
            foreach (var tenantName in new[] { "tenant-a", "tenant-b" })
            {
                var tenant = await EnsureTenantAsync(tenantName);
                using (_currentTenant.Change(tenant.Id, tenant.Name))
                {
                    await SeedContextAsync(tenant.Id);
                }
            }
        }
    }

    private async Task<Tenant> EnsureTenantAsync(string name)
    {
        var existing = await _tenantRepository.FindByNameAsync(name);
        if (existing != null)
        {
            return existing;
        }

        var tenant = await _tenantManager.CreateAsync(name);
        await _tenantRepository.InsertAsync(tenant, autoSave: true);
        return tenant;
    }

    private async Task SeedContextAsync(Guid? tenantId)
    {
        var user1 = await EnsureUserAsync("alice", tenantId);
        var user2 = await EnsureUserAsync("bob", tenantId);

        await EnsureDocumentAsync("Alice private notes", user1.Id, tenantId);
        await EnsureDocumentAsync("Bob private notes", user2.Id, tenantId);
    }

    private async Task<IdentityUser> EnsureUserAsync(string userName, Guid? tenantId)
    {
        var existing = await _userManager.FindByNameAsync(userName);
        if (existing != null)
        {
            return existing;
        }

        var user = new IdentityUser(
            _guidGenerator.Create(),
            userName,
            userName + "@abpgoat.local",
            tenantId);

        var result = await _userManager.CreateAsync(user, DefaultPassword);
        if (!result.Succeeded)
        {
            throw new AbpException(
                $"Failed to seed user '{userName}': " +
                string.Join(", ", result.Errors.Select(e => e.Description)));
        }

        return user;
    }

    private async Task EnsureDocumentAsync(string title, Guid ownerUserId, Guid? tenantId)
    {
        var queryable = await _documentRepository.GetQueryableAsync();
        if (queryable.Any(d => d.Title == title && d.OwnerUserId == ownerUserId))
        {
            return;
        }

        await _documentRepository.InsertAsync(
            new Document(_guidGenerator.Create(), title, "seed.txt", ownerUserId, tenantId),
            autoSave: true);
    }
}
