using Logic.Models;

namespace Api.DTOs.Invoice;

public class InvoiceDiscountEditDto
{
    public Guid? Id { get; set; }
    public string Description { get; set; } = String.Empty;
    public decimal Amount { get; set; }
    public InvoiceDiscountType Type { get; set; }
    public Guid? InvoiceId { get; set; }
}
