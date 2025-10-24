using Logic.Models;

namespace Api.DTOs.Business;

public class BusinessDto
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public string? Tagline { get; set; }
    public required string Address { get; set; }
    public required string Phone { get; set; }
    public required string Email { get; set; }
    public string? Website { get; set; }
    public CurrencyCode DefaultCurrency { get; set; }
    public string? DefaultPaymentInstructions { get; set; }
}
