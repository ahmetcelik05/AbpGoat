namespace AbpGoat.Vulnerable.Documents;

/// <summary>
/// Where uploaded document files are written on disk. Configured in the Application module.
/// </summary>
public class DocumentStorageOptions
{
    public string BasePath { get; set; } = "BlobStoring";
}
