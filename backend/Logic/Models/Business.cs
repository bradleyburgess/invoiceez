using System.ComponentModel.DataAnnotations;
using Logic.Utils.Validations;

namespace Logic.Models;

public class Business
{
    [Key]
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public string? Tagline { get; set; }
    [ValidEmailAddress]
    public required string Email { get; set; }
    public required string Phone { get; set; }
    public required string Address { get; set; }
    public string? Website { get; set; }
    public Guid UserId { get; set; }
    public User? User { get; set; }
    public CurrencyCode DefaultCurrency { get; set; }
    public string? DefaultPaymentInstructions { get; set; }

    public ICollection<Invoice> Invoices { get; set; } = [];
}
