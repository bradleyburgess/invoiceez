namespace Logic.Models;

public class Address
{
    public Guid Id { get; set; }
    public string Street1 { get; set; } = String.Empty;
    public string Street2 { get; set; } = String.Empty;
    public string Street3 { get; set; } = String.Empty;
    public string City { get; set; } = String.Empty;
    public string PostalCode { get; set; } = String.Empty;
    public string State { get; set; } = String.Empty;
    public string Country { get; set; } = String.Empty;
    public Guid CustomerId { get; set; }
    public Customer? Customer { get; set; }
}
