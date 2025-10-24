using System.ComponentModel.DataAnnotations;

namespace Logic.Models;

public class InvoiceDiscount
{
    [Key]
    public Guid Id { get; set; }
    public string Description { get; set; } = String.Empty;
    public decimal Amount { get; set; }
    public InvoiceDiscountType Type { get; set; }
    public Guid InvoiceId { get; set; }
    public Invoice? Invoice { get; set; }
}

public enum InvoiceDiscountType {
    Fixed,
    Percentage,
}