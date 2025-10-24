using Api.DTOs.Invoice;
using Logic.Models;

namespace Api.Extensions.Mapping;

public static class InvoiceItemMappingExtensions
{
    public static InvoiceItemDto MapToDto(this InvoiceItem item) =>
        new InvoiceItemDto
        {
            Id = item.Id,
            Description = item.Description,
            Quantity = item.Quantity,
            Rate = item.Rate,
            InvoiceId = item.InvoiceId,
        };

    public static InvoiceItem MapToEntity(this InvoiceItemEditDto itemDto) =>
        new InvoiceItem
        {
            Description = itemDto.Description,
            Quantity = itemDto.Quantity,
            Rate = itemDto.Rate,
        };

}
