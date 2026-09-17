using System;
using System.Security.Cryptography;
using System.Text;
using Volo.Abp.DependencyInjection;

namespace AbpGoat.Vulnerable.Security;

/// <summary>
/// Legacy password hashing. Deliberately insecure — see VULNERABILITIES.md (VL-006).
/// </summary>
public class LegacyHasher : ITransientDependency
{
    // VL-006 (CWE-327): MD5 is fast and broken for password storage — no salt, no work factor.
    // A safe implementation would use a memory-hard KDF such as PBKDF2, bcrypt, or Argon2.
    public string HashPassword(string password)
    {
        var bytes = MD5.HashData(Encoding.UTF8.GetBytes(password));
        return Convert.ToHexString(bytes);
    }
}
