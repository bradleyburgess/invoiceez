using Api.DTOs.Business;
using Api.DTOs.Invoice;
using Logic.Models;
using Microsoft.Extensions.Configuration.UserSecrets;

namespace Api.Extensions.Mapping;

public static class BusinessMappingExtensions
{
    public static BusinessDto MapToDto(this Business business)
    {
        return new BusinessDto
        {
            Id = business.Id,
            Name = business.Name,
            Tagline = business.Tagline,
            Address = business.Address,
            Phone = business.Phone,
            Email = business.Email,
            Website = business.Website,
            DefaultCurrency = business.DefaultCurrency,
            DefaultPaymentInstructions = business.DefaultPaymentInstructions,
        };
    }

    public static Business MapToEntity(this BusinessEditDto dto, Guid userId) =>
    new Business
    {
        Name = dto.Name,
        Tagline = dto.Tagline,
        Address = dto.Address,
        Phone = dto.Phone,
        Email = dto.Email,
        Website = dto.Website,
        UserId = userId,
        DefaultCurrency = dto.DefaultCurrency,
        DefaultPaymentInstructions = dto.DefaultPaymentInstructions,
    };

    public static Business MapToBusinessEntity(this InvoiceEditDto dto, Guid userId) =>
        new Business
        {
            Name = dto.BusinessName,
            Tagline = dto.BusinessTagline,
            Email = dto.BusinessEmail,
            Phone = dto.BusinessPhone,
            Address = dto.BusinessAddress,
            Website = dto.BusinessWebsite,
            DefaultCurrency = dto.Currency,
            DefaultPaymentInstructions = dto.PaymentInstructions,
            UserId = userId,
        };


    public static void UpdateFromDto(this Business business, BusinessEditDto dto)
    {
        business.Name = dto.Name;
        business.Tagline = dto.Tagline;
        business.Address = dto.Address;
        business.Phone = dto.Phone;
        business.Email = dto.Email;
        business.Website = dto.Website;
        business.DefaultCurrency = dto.DefaultCurrency;
        business.DefaultPaymentInstructions = dto.DefaultPaymentInstructions;
    }

    public static void UpdateFromInvoiceEditDto(this Business business, InvoiceEditDto dto)
    {
        business.Name = dto.BusinessName;
        business.Tagline = dto.BusinessTagline;
        business.Address = dto.BusinessAddress;
        business.Email = dto.BusinessEmail;
        business.Phone = dto.BusinessPhone;
        business.Website = dto.BusinessWebsite;
        business.DefaultPaymentInstructions = dto.PaymentInstructions;
    }
}
