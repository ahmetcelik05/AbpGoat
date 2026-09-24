using System;
using System.Security.Cryptography;
using System.Text;
using Volo.Abp.DependencyInjection;

namespace AbpGoat.Vulnerable.Security;

/// <summary>
/// Legacy password hashing helper retained for backwards compatibility.
/// </summary>
public class LegacyHasher : ITransientDependency
{
    public string HashPassword(string password)
    {
        var bytes = MD5.HashData(Encoding.UTF8.GetBytes(password));
        return Convert.ToHexString(bytes);
    }
}
