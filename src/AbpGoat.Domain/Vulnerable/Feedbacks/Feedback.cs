using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace AbpGoat.Vulnerable.Feedbacks;

/// <summary>
/// A public feedback message. Deliberately insecure — see VULNERABILITIES.md (VL-002).
/// The message is stored verbatim and later rendered unescaped, enabling stored XSS.
/// </summary>
public class Feedback : CreationAuditedAggregateRoot<Guid>
{
    public string AuthorName { get; private set; }

    public string Message { get; private set; }

    protected Feedback()
    {
        AuthorName = string.Empty;
        Message = string.Empty;
    }

    public Feedback(Guid id, string authorName, string message)
        : base(id)
    {
        AuthorName = authorName;
        Message = message;
    }
}
