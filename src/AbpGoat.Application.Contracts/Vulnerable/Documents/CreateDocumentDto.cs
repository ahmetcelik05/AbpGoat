using System.ComponentModel.DataAnnotations;

namespace AbpGoat.Vulnerable.Documents;

public class CreateDocumentDto
{
    [Required]
    [StringLength(256)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [StringLength(256)]
    public string FileName { get; set; } = string.Empty;

    [Required]
    public byte[] Content { get; set; } = System.Array.Empty<byte>();
}
