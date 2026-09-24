namespace AbpGoat.Vulnerable.Billing;

public class SavePaymentDto
{
    public string CardNumber { get; set; } = string.Empty;

    public string Cvv { get; set; } = string.Empty;

    public decimal Amount { get; set; }
}
