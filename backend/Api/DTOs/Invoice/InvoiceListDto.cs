namespace Api.DTOs.Invoice;

public class InvoiceListDto
{
    public List<InvoiceSummaryDto> Invoices { get; set; } = [];
}
