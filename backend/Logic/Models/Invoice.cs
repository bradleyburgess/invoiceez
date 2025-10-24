using Logic.Utils.Validations;
using System.ComponentModel.DataAnnotations;

namespace Logic.Models;

public class Invoice
{
    [Key]
    public Guid Id { get; set; }
    public required string InvoiceNumber { get; set; }
    public required DateTime InvoiceDate { get; set; }
    public InvoicePaymentStatus PaymentStatus { get; set; } = InvoicePaymentStatus.Unpaid;
    public decimal TotalAmount { get; set; }
    public CurrencyCode Currency { get; set; }
    public string? PaymentInstructions { get; set; }
    public ICollection<InvoiceItem> Items { get; set; } = [];
    public ICollection<InvoiceDiscount> Discounts { get; set; } = [];

    public Guid? BusinessId { get; set; }
    public Business? Business { get; set; }
    public required string BusinessName { get; set; }
    public string? BusinessTagline { get; set; }
    public required string BusinessAddress { get; set; }
    public required string BusinessEmail { get; set; }
    public required string BusinessPhone { get; set; }
    public string? BusinessWebsite { get; set; }

    public Guid? CustomerId { get; set; }
    public Customer? Customer { get; set; }
    public required string CustomerName { get; set; }
    public required string CustomerAddress { get; set; }
    [ValidEmailAddress]
    public required string CustomerEmail { get; set; }
    public required string CustomerPhone { get; set; }

    public DateTime CreatedAtUtc { get; set; } = DateTime.Now;
    public DateTime ModifiedAtUtc { get; set; } = DateTime.Now;

    public Guid UserId { get; set; }
    public User? User { get; set; }
}
