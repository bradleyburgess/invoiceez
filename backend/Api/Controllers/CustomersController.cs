using Api.DTOs;
using Api.DTOs.Customer;
using Api.Extensions.Mapping;
using Api.Services;
using Logic.Database;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CustomersController(
    AppDbContext dbContext,
    IUserContextService userContextService
) : ControllerBase
{
    [HttpGet]
    [SwaggerOperation(OperationId = "GetCustomers", Summary = "Get all customers for the authenticated user")]
    public async Task<ActionResult<ApiResponse<IEnumerable<CustomerDto>>>> GetCustomers()
    {
        var userResult = await userContextService.GetUserAsync();
        if (!userResult.IsSuccess)
        {
            return NotFound(ApiResponse<IEnumerable<CustomerDto>>.Fail(
                ApiResponseCode.NotFound,
                userResult.ErrorMessage
            ));
        }
        var user = userResult.Value!;

        var customers = await dbContext.Customers
            .Where(c => c.UserId == user.Id)
            .Select(b => b.MapToDto())
            .ToListAsync();
        return Ok(ApiResponse<IEnumerable<CustomerDto>>.Ok(data: customers));
    }

    [HttpGet("{id:guid}")]
    [SwaggerOperation(OperationId = "GetCustomer", Summary = "Get a customer by ID for the authenticated user")]
    public async Task<ActionResult<ApiResponse<CustomerDto>>> GetCustomer(Guid id)
    {
        var userIdResult = userContextService.GetUserId();
        var customer = await dbContext.Customers
            .Where(c => c.Id == id && c.UserId == userIdResult.Value)
            .Select(c => c.MapToDto())
            .FirstOrDefaultAsync();
        if (customer == null)
        {
            return NotFound(ApiResponse<CustomerDto>.Fail(
                ApiResponseCode.NotFound,
                "Customer not found"
            ));
        }
        return Ok(ApiResponse<CustomerDto>.Ok(customer));
    }

    [HttpPost]
    [SwaggerOperation(OperationId = "CreateCustomer", Summary = "Create a new customer for the authenticated user")]
    public async Task<ActionResult<ApiResponse<CustomerDto>>> CreateCustomer(CustomerEditDto dto)
    {
        var userResult = await userContextService.GetUserAsync();
        if (!userResult.IsSuccess || userResult.Value == null)
        {
            return NotFound(ApiResponse<CustomerDto>.Fail(
                ApiResponseCode.NotFound,
                "User not found"
            ));
        }
        var customer = dto.MapToEntity(userResult.Value.Id);
        dbContext.Customers.Add(customer);
        await dbContext.SaveChangesAsync();
        var customerDto = customer.MapToDto();
        return CreatedAtAction(
            nameof(GetCustomer),
            new { id = customer.Id },
            ApiResponse<CustomerDto>.Ok(customerDto)
        );
    }

    [HttpDelete("{id:guid}")]
    [SwaggerOperation(OperationId = "DeleteCustomer", Summary = "Delete a customer by ID for the authenticated user")]
    public async Task<ActionResult<ApiResponse<EmptyDto>>> DeleteCustomer(Guid id)
    {
        var userIdResult = userContextService.GetUserId();
        var customer = await dbContext.Customers
            .Where(c => c.Id == id && c.UserId == userIdResult.Value)
            .FirstOrDefaultAsync();
        if (customer == null)
        {
            return NotFound(ApiResponse<EmptyDto>.Fail(
                ApiResponseCode.NotFound,
                "Customer not found"
            ));
        }
        dbContext.Customers.Remove(customer);
        await dbContext.SaveChangesAsync();
        return Ok(ApiResponse<EmptyDto>.Ok(message: "Customer deleted successfully"));
    }

    [HttpPut("{id:guid}")]
    [SwaggerOperation(OperationId = "UpdateCustomer", Summary = "Update a customer by ID for the authenticated user")]
    public async Task<ActionResult<ApiResponse<CustomerDto>>> UpdateCustomer(Guid id, CustomerEditDto dto)
    {
        var userIdResult = userContextService.GetUserId();
        var customer = await dbContext.Customers
            .Where(c => c.Id == id && c.UserId == userIdResult.Value)
            .FirstOrDefaultAsync();
        if (customer == null)
        {
            return NotFound(ApiResponse<CustomerDto>.Fail(
                ApiResponseCode.NotFound,
                "Customer not found"
            ));
        }
        customer.UpdateFromDto(dto);
        await dbContext.SaveChangesAsync();
        var customerDto = customer.MapToDto();
        return Ok(ApiResponse<CustomerDto>.Ok(customerDto, "Customer updated successfully"));
    }
}
