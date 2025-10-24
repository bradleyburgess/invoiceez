using Api.DTOs.Customer;
using Api.DTOs.Invoice;
using Logic.Models;

namespace Api.Extensions.Mapping;

public static class CustomerMappingExtensions
{
    public static Customer MapToEntity(this CustomerEditDto dto, Guid userId) =>
        new Customer()
        {
            Name = dto.Name,
            Address = dto.Address,
            Email = dto.Email,
            Phone = dto.Phone,
            UserId = userId,
        };

    public static Customer MapToCustomerEntity(this InvoiceEditDto dto, Guid userId) =>
        new Customer()
        {
            Name = dto.CustomerName,
            Address = dto.CustomerAddress,
            Email = dto.CustomerEmail,
            Phone = dto.CustomerPhone,
            UserId = userId,
        };

    public static CustomerDto MapToDto(this Customer customer) =>
        new CustomerDto()
        {
            Id = customer.Id,
            Name = customer.Name,
            Address = customer.Address,
            Email = customer.Email,
            Phone = customer.Phone,
        };

    public static void UpdateFromDto(this Customer customer, CustomerEditDto dto)
    {
        customer.Name = dto.Name;
        customer.Address = dto.Address;
        customer.Email = dto.Email;
        customer.Phone = dto.Phone;
    }

    public static void UpdateFromInvoiceEditDto(this Customer customer, InvoiceEditDto dto)
    {
        customer.Name = dto.CustomerName;
        customer.Address = dto.CustomerAddress;
        customer.Email = dto.CustomerEmail;
        customer.Phone = dto.CustomerPhone;
    }
}
