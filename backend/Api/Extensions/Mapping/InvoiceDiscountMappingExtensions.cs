using Api.DTOs.Invoice;
using Logic.Models;

namespace Api.Extensions.Mapping;

public static class InvoiceDiscountMappingExtensions
{
    public static InvoiceDiscountDto MapToDto(this InvoiceDiscount discount) =>
        new InvoiceDiscountDto
        {
            Id = discount.Id,
            Description = discount.Description,
            Amount = discount.Amount,
            InvoiceId = discount.InvoiceId,
            Type = discount.Type,
        };

    public static InvoiceDiscount MapToEntity(this InvoiceDiscountEditDto discountDto) =>
        new InvoiceDiscount
        {
            Description = discountDto.Description,
            Amount = discountDto.Amount,
            Type = discountDto.Type,
        };

}
