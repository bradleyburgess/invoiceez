using Logic.Models;

namespace Api.DTOs.Invoice;

public class InvoiceSummaryDto
{
    public Guid Id { get; set; }
    public string InvoiceNumber { get; set; } = String.Empty;
    public DateTime InvoiceDate { get; set; } = DateTime.Now;
    public InvoicePaymentStatus PaymentStatus { get; set; } = InvoicePaymentStatus.Unpaid;
    public decimal TotalAmount { get; set; }
    public int ItemCount { get; set; }
    public int DiscountCount { get; set; }
    public CurrencyCode Currency { get; set; }

    public Guid? CustomerId { get; set; }
    public required string CustomerName { get; set; }

    public Guid? BusinessId { get; set; }
    public string? BusinessName { get; set; } = String.Empty;

    public Guid UserId { get; set; }

    public DateTime CreatedAtUtc { get; set; } = DateTime.Now;
    public DateTime ModifiedAtUtc { get; set; } = DateTime.Now;
}
