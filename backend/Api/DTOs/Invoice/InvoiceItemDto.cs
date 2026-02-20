namespace Api.DTOs.Invoice;

public class InvoiceItemDto
{
    public Guid Id { get; set; }
    public string Description { get; set; } = String.Empty;
    public decimal Quantity { get; set; }
    public decimal Rate { get; set; }
    public Guid InvoiceId { get; set; }
    public int Order { get; set; } = 0;
}
