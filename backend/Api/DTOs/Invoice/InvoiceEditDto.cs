using Logic.Models;
using Logic.Utils.Validations;

namespace Api.DTOs.Invoice;

public class InvoiceEditDto
{
    public Guid? Id { get; set; }
    public string InvoiceNumber { get; set; } = String.Empty;
    public DateTime InvoiceDate { get; set; } = DateTime.Now;
    public InvoicePaymentStatus PaymentStatus { get; set; } = InvoicePaymentStatus.Unpaid;
    public decimal TotalAmount { get; set; }
    public IEnumerable<InvoiceItemEditDto> Items { get; set; } = [];
    public IEnumerable<InvoiceDiscountEditDto> Discounts { get; set; } = [];
    public CurrencyCode Currency { get; set; }
    public string? PaymentInstructions { get; set; }

    public Guid? BusinessId { get; set; }
    public required string BusinessName { get; set; }
    public string? BusinessTagline { get; set; }
    public required string BusinessAddress { get; set; }
    [ValidEmailAddress]
    public required string BusinessEmail { get; set; }
    public required string BusinessPhone { get; set; }
    public string? BusinessWebsite { get; set; }
    public bool ShouldSaveBusiness { get; set; }

    public Guid? CustomerId { get; set; }
    public required string CustomerName { get; set; }
    public required string CustomerAddress { get; set; }
    public required string CustomerEmail { get; set; }
    public required string CustomerPhone { get; set; }
    public bool ShouldSaveCustomer { get; set; }
}
