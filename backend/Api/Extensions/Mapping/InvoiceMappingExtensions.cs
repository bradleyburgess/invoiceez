using Api.DTOs.Invoice;
using Logic.Models;

namespace Api.Extensions.Mapping;

public static class InvoiceMappingExtensions
{
    public static InvoiceSummaryDto MapToSummaryDto(this Invoice invoice) =>
        new InvoiceSummaryDto
        {
            Id = invoice.Id,
            InvoiceNumber = invoice.InvoiceNumber,
            InvoiceDate = invoice.InvoiceDate,
            PaymentStatus = invoice.PaymentStatus,
            TotalAmount = invoice.TotalAmount,
            ItemCount = invoice.Items.Count,
            DiscountCount = invoice.Discounts.Count,
            Currency = invoice.Currency,

            BusinessId = invoice.BusinessId,
            BusinessName = invoice.Business!.Name,

            CustomerId = invoice.CustomerId,
            CustomerName = invoice.CustomerName,
            CreatedAtUtc = invoice.CreatedAtUtc,

            ModifiedAtUtc = invoice.ModifiedAtUtc,
            UserId = invoice.UserId,
        };

    public static InvoiceDetailDto MapToDetailDto(this Invoice invoice) =>
        new InvoiceDetailDto
        {
            Id = invoice.Id,
            InvoiceNumber = invoice.InvoiceNumber,
            InvoiceDate = invoice.InvoiceDate,
            PaymentStatus = invoice.PaymentStatus,
            TotalAmount = invoice.TotalAmount,
            Items = invoice.Items?.OrderBy(i => i.Order).Select(item => item.MapToDto()) ?? [],
            Discounts = invoice.Discounts?.Select(discount => discount.MapToDto()) ?? [],
            PaymentInstructions = invoice.PaymentInstructions,
            Currency = invoice.Currency,

            BusinessId = invoice.BusinessId,
            BusinessName = invoice.BusinessName,
            BusinessTagline = invoice.BusinessTagline,
            BusinessAddress = invoice.BusinessAddress,
            BusinessEmail = invoice.BusinessEmail,
            BusinessPhone = invoice.BusinessPhone,
            BusinessWebsite = invoice.BusinessWebsite,

            CustomerId = invoice.CustomerId,
            CustomerName = invoice.CustomerName,
            CustomerAddress = invoice.CustomerAddress,
            CustomerEmail = invoice.CustomerEmail,
            CustomerPhone = invoice.CustomerPhone,

            CreatedAtUtc = invoice.CreatedAtUtc,
            ModifiedAtUtc = invoice.ModifiedAtUtc,
        };

    public static Invoice MapToEntity(this InvoiceEditDto dto, Guid userId) =>
        new Invoice
        {
            InvoiceNumber = dto.InvoiceNumber,
            PaymentStatus = dto.PaymentStatus,
            InvoiceDate = dto.InvoiceDate,
            TotalAmount = dto.TotalAmount,
            Items = dto.Items.Select(itemDto => itemDto.MapToEntity()).ToList() ?? [],
            Discounts = dto.Discounts?.Select(discountDto => discountDto.MapToEntity()).ToList() ?? [],
            PaymentInstructions = dto.PaymentInstructions,
            Currency = dto.Currency,

            BusinessId = dto.BusinessId,
            BusinessName = dto.BusinessName,
            BusinessTagline = dto.BusinessTagline,
            BusinessAddress = dto.BusinessAddress,
            BusinessEmail = dto.BusinessEmail,
            BusinessPhone = dto.BusinessPhone,
            BusinessWebsite = dto.BusinessWebsite,

            CustomerId = dto.CustomerId,
            CustomerName = dto.CustomerName,
            CustomerAddress = dto.CustomerAddress,
            CustomerEmail = dto.CustomerEmail,
            CustomerPhone = dto.CustomerPhone,

            UserId = userId,

            CreatedAtUtc = DateTime.UtcNow,
            ModifiedAtUtc = DateTime.UtcNow,
        };

    public static void UpdateFromDto(this Invoice invoice, InvoiceEditDto dto, bool updateTimestamp = true)
    {
        invoice.InvoiceNumber = dto.InvoiceNumber;
        invoice.InvoiceDate = dto.InvoiceDate;
        invoice.PaymentStatus = dto.PaymentStatus;
        invoice.TotalAmount = dto.TotalAmount;

        invoice.BusinessId = dto.BusinessId;
        invoice.BusinessName = dto.BusinessName;
        invoice.BusinessTagline = dto.BusinessTagline;
        invoice.BusinessAddress = dto.BusinessAddress;
        invoice.BusinessEmail = dto.BusinessEmail;
        invoice.BusinessPhone = dto.BusinessPhone;
        invoice.BusinessWebsite = dto.BusinessWebsite;

        invoice.CustomerId = dto.CustomerId;
        invoice.CustomerName = dto.CustomerName;
        invoice.CustomerAddress = dto.CustomerAddress;
        invoice.CustomerEmail = dto.CustomerEmail;
        invoice.CustomerPhone = dto.CustomerPhone;

        if (updateTimestamp) invoice.ModifiedAtUtc = DateTime.UtcNow;
        invoice.Currency = dto.Currency;
        invoice.PaymentInstructions = dto.PaymentInstructions;

        // Update Items
        invoice.Items.Clear();
        if (dto.Items != null)
        {
            foreach (var itemDto in dto.Items)
            {
                var item = itemDto.MapToEntity();
                if (itemDto.Id != null && itemDto.Id != Guid.Empty)
                    item.Id = itemDto.Id.Value;
                invoice.Items.Add(item);
            }
        }

        // Update Discounts
        invoice.Discounts.Clear();
        if (dto.Discounts != null)
        {
            foreach (var discountDto in dto.Discounts)
            {
                var discount = discountDto.MapToEntity();
                if (discountDto.Id != null && discountDto.Id != Guid.Empty)
                    discount.Id = discountDto.Id.Value;
                invoice.Discounts.Add(discount);
            }
        }
    }
}
