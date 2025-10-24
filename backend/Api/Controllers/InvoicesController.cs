using System.Text;
using Api.DTOs;
using Api.DTOs.Invoice;
using Api.Extensions.Mapping;
using Api.Services;
using Logic.Database;
using Logic.Extensions;
using Logic.Models;
using Logic.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class InvoicesController(
    AppDbContext dbContext,
    IUserContextService userContextService,
    IInvoiceGenerationService invoiceGenerationService
)
    : ControllerBase
{
    [HttpGet("for-me")]
    [SwaggerOperation(
        OperationId = "GetUserInvoices",
        Summary = "Get all invoices for the authenticated user"
    )]
    public async Task<ActionResult<ApiResponse<InvoiceListDto>>> GetUserInvoices()
    {
        var userResult = await userContextService.GetUserAsync();
        if (!userResult.IsSuccess)
        {
            return NotFound(ApiResponse<InvoiceListDto>.Fail(
                ApiResponseCode.NotFound,
                userResult.ErrorMessage
            ));
        }
        var user = userResult.Value!;
        var invoices = await dbContext.Invoices
            .Where(i => i.UserId == user.Id)
            .Include(i => i.Business)
            .Include(i => i.Items)
            .Include(i => i.Discounts)
            .Select(i => i.MapToSummaryDto())
            .ToListAsync();

        return Ok(ApiResponse<InvoiceListDto>.Ok(
            new InvoiceListDto
            {
                Invoices = invoices
            }
        ));
    }

    [HttpGet("for-business/{id:guid}")]
    [SwaggerOperation(
        OperationId = "GetBusinessInvoices",
        Summary = "Get all invoices for a specific business owned by the authenticated user"
    )]
    public async Task<ActionResult<ApiResponse<InvoiceListDto>>> GetBusinessInvoices(Guid id)
    {
        var userIdResult = userContextService.GetUserId();
        if (userIdResult == null)
        {
            return Unauthorized(ApiResponse<InvoiceListDto>.Fail(
                ApiResponseCode.Unauthorized,
                "User not authenticated"
            ));
        }
        var business = await dbContext.Businesses
            .Where(b => b.UserId == userIdResult.Value)
            .FirstOrDefaultAsync(b => b.Id == id);
        if (business == null)
        {
            return NotFound(ApiResponse<InvoiceListDto>.Fail(
                ApiResponseCode.NotFound,
                "Business not found"
            ));
        }

        var invoices = await dbContext.Invoices
            .Where(i => i.BusinessId == business.Id)
            .Select(i => i.MapToSummaryDto())
            .ToListAsync();

        return Ok(ApiResponse<InvoiceListDto>.Ok(
            new InvoiceListDto
            {
                Invoices = invoices
            }
        ));
    }

    [HttpGet("{id:guid}")]
    [SwaggerOperation(
        OperationId = "GetInvoiceById",
        Summary = "Get a specific invoice by its ID"
    )]
    public async Task<ActionResult<ApiResponse<InvoiceDetailDto>>> GetInvoiceById(Guid id)
    {
        var userIdResult = userContextService.GetUserId();
        if (userIdResult == null)
        {
            return Unauthorized(ApiResponse<InvoiceDetailDto>.Fail(
                ApiResponseCode.Unauthorized,
                "User not authenticated"
            ));
        }
        var invoice = await dbContext.Invoices
            .Include(i => i.Items)
            .Include(i => i.Discounts)
            .Include(i => i.Business)
            .FirstOrDefaultAsync(i => i.Id == id && i.UserId == userIdResult.Value);
        if (invoice == null)
        {
            return NotFound(ApiResponse<InvoiceDetailDto>.Fail(
                ApiResponseCode.NotFound,
                "Invoice not found"
            ));
        }
        return Ok(ApiResponse<InvoiceDetailDto>.Ok(
            invoice.MapToDetailDto()
        ));
    }

    [HttpPost]
    [SwaggerOperation(
        OperationId = "CreateInvoice",
        Summary = "Create a new invoice for the authenticated user"
    )]
    public async Task<ActionResult<ApiResponse<InvoiceDetailDto>>> CreateInvoice([FromBody] InvoiceEditDto dto)
    {
        var userIdResult = userContextService.GetUserId();
        if (userIdResult == null)
        {
            return Unauthorized(ApiResponse<InvoiceDetailDto>.Fail(
                ApiResponseCode.Unauthorized,
                "User not authenticated"
            ));
        }
        if (dto.ShouldSaveBusiness)
        {
            var business = await dbContext.Businesses
                .Where(b => b.UserId == userIdResult.Value)
                .FirstOrDefaultAsync(b => b.Id == dto.BusinessId);

            if (business == null)
            {
                business = dto.MapToBusinessEntity(userIdResult.Value);
                dbContext.Businesses.Add(business);
                await dbContext.SaveChangesAsync();
                dto.BusinessId = business.Id;
            }
            else business.UpdateFromInvoiceEditDto(dto);
        }


        if (dto.ShouldSaveCustomer)
        {
            var customer = await dbContext.Customers
                .Where(c => c.UserId == userIdResult.Value)
                .FirstOrDefaultAsync(c => c.Id == dto.CustomerId);

            if (customer == null)
            {
                customer = dto.MapToCustomerEntity(userIdResult.Value);
                dbContext.Customers.Add(customer);
                await dbContext.SaveChangesAsync();
                dto.CustomerId = customer.Id;
            }
            else customer.UpdateFromInvoiceEditDto(dto);
        }

        var newInvoice = dto.MapToEntity(userIdResult.Value);
        newInvoice.TotalAmount = newInvoice.CalculateTotalAmount();
        dbContext.Invoices.Add(newInvoice);
        await dbContext.SaveChangesAsync();

        var createdInvoice = await dbContext.Invoices
            .Include(i => i.Items)
            .Include(i => i.Discounts)
            .Include(i => i.Business)
            .FirstOrDefaultAsync(i => i.Id == newInvoice.Id);

        return Ok(ApiResponse<InvoiceDetailDto>.Ok(
            createdInvoice!.MapToDetailDto()
        ));
    }

    [HttpPut("{id:guid}")]
    [SwaggerOperation(
        OperationId = "UpdateInvoice",
        Summary = "Update an existing invoice by its ID"
    )]
    public async Task<ActionResult<ApiResponse<InvoiceDetailDto>>> UpdateInvoice(Guid id, [FromBody] InvoiceEditDto dto)
    {
        var userIdResult = userContextService.GetUserId();
        if (userIdResult == null)
        {
            return Unauthorized(ApiResponse<InvoiceDetailDto>.Fail(
                ApiResponseCode.Unauthorized,
                "User not authenticated"
            ));
        }

        var invoice = await dbContext.Invoices
            .Include(i => i.Business)
            .Include(i => i.Customer)
            .Include(i => i.Items)
            .Include(i => i.Discounts)
            .FirstOrDefaultAsync(i => i.Id == id && i.UserId == userIdResult.Value);
        if (invoice == null)
        {
            return NotFound(ApiResponse<InvoiceDetailDto>.Fail(
                ApiResponseCode.NotFound,
                "Invoice not found"
            ));
        }

        if (dto.ShouldSaveBusiness)
        {
            var business = await dbContext.Businesses
                .Where(b => b.UserId == userIdResult.Value)
                .FirstOrDefaultAsync(b => b.Id == dto.BusinessId);

            if (business == null)
            {
                business = dto.MapToBusinessEntity(userIdResult.Value);
                dbContext.Businesses.Add(business);
                await dbContext.SaveChangesAsync();
                dto.BusinessId = business.Id;
            }
            else business.UpdateFromInvoiceEditDto(dto);
        }

        if (dto.ShouldSaveCustomer)
        {
            var customer = await dbContext.Customers
                .Where(c => c.UserId == userIdResult.Value)
                .FirstOrDefaultAsync(c => c.Id == dto.CustomerId);

            if (customer == null)
            {
                customer = dto.MapToCustomerEntity(userIdResult.Value);
                dbContext.Customers.Add(customer);
                await dbContext.SaveChangesAsync();
                dto.CustomerId = customer.Id;
            }
            else customer.UpdateFromInvoiceEditDto(dto);
        }

        invoice.UpdateFromDto(dto);
        invoice.TotalAmount = invoice.CalculateTotalAmount();
        dbContext.Invoices.Update(invoice);
        await dbContext.SaveChangesAsync();
        return Ok(ApiResponse<InvoiceDetailDto>.Ok(
            invoice.MapToDetailDto()
        ));
    }

    [HttpDelete("{id:guid}")]
    [SwaggerOperation(
        OperationId = "DeleteInvoice",
        Summary = "Delete an invoice by its ID"
    )]
    public async Task<ActionResult<ApiResponse<EmptyDto>>> DeleteInvoice(Guid id)
    {
        var userIdResult = userContextService.GetUserId();
        if (userIdResult == null)
        {
            return Unauthorized(ApiResponse<EmptyDto>.Fail(
                ApiResponseCode.Unauthorized,
                "User not authenticated"
            ));
        }
        var invoice = await dbContext.Invoices
            .FirstOrDefaultAsync(i => i.Id == id && i.UserId == userIdResult.Value);
        if (invoice == null)
        {
            return NotFound(ApiResponse<EmptyDto>.Fail(
                ApiResponseCode.NotFound,
                "Invoice not found"
            ));
        }
        dbContext.Invoices.Remove(invoice);
        await dbContext.SaveChangesAsync();
        return Ok(ApiResponse<EmptyDto>.Ok(message: "Invoice deleted successfully"));
    }

    [HttpPut("update-status/{id:guid}")]
    [SwaggerOperation(
        OperationId = "UpdateInvoiceStatus",
        Summary = "Update the payment status of an invoice by its ID"
    )]
    public async Task<ActionResult<ApiResponse<InvoiceSummaryDto>>> UpdateInvoiceStatus(Guid id, [FromBody] InvoicePaymentStatus status)
    {
        var userIdResult = userContextService.GetUserId();
        if (userIdResult == null)
        {
            return Unauthorized(ApiResponse<InvoiceDetailDto>.Fail(
                ApiResponseCode.Unauthorized,
                "User not authenticated"
            ));
        }
        var invoice = await dbContext.Invoices
            .Include(i => i.Items)
            .Include(i => i.Discounts)
            .Include(i => i.Business)
            .FirstOrDefaultAsync(i => i.Id == id && i.UserId == userIdResult.Value);
        if (invoice == null)
        {
            return NotFound(ApiResponse<InvoiceDetailDto>.Fail(
                ApiResponseCode.NotFound,
                "Invoice not found"
            ));
        }
        invoice.PaymentStatus = status;
        dbContext.Invoices.Update(invoice);
        await dbContext.SaveChangesAsync();
        return Ok(ApiResponse<InvoiceSummaryDto>.Ok(
            invoice.MapToSummaryDto()
        ));
    }

    [HttpGet("generate-invoice-number")]
    [SwaggerOperation(
        OperationId = "GenerateInvoiceNumber",
        Summary = "Generate an invoice number from a given date"
    )]
    public async Task<ActionResult<ApiResponse<string>>> GenerateInvoiceNumber([FromQuery] DateTime InvoiceDate)
    {
        var numInvoiceOnDate = await dbContext.Invoices
            .Where(i => i.InvoiceNumber.StartsWith("INV"))
            .Where(i => i.InvoiceDate.Date == InvoiceDate.Date)
            .CountAsync();
        return Ok(ApiResponse<string>.Ok(data: GetAutoGeneratedInvoiceNumber(InvoiceDate, numInvoiceOnDate)));
    }

    [HttpGet("validate-invoice-number")]
    [SwaggerOperation(
        OperationId = "ValidateInvoiceNumber",
        Summary = "Validate an invoice number, checking for uniqueness to the user"
    )]
    public async Task<ActionResult<ApiResponse<bool>>> ValidateInvoiceNumber([FromQuery] string InvoiceNumber, [FromQuery] Guid? InvoiceId)
    {
        var userIdResult = userContextService.GetUserId();
        if (!userIdResult.IsSuccess)
            return Unauthorized(ApiResponse<bool>.Fail(
                ApiResponseCode.Unauthorized,
                "User not authenticated"));
        var existingInvoice = await dbContext.Invoices
            .FirstOrDefaultAsync(i =>
                i.UserId == userIdResult.Value &&
                i.InvoiceNumber == InvoiceNumber &&
                i.Id != InvoiceId);

        if (existingInvoice == null) return Ok(ApiResponse<bool>.Ok(data: true));
        return Ok(ApiResponse<bool>.Ok(data: false, message: "Invoice with that number already exists"));
    }

    [HttpGet("generate-invoice-pdf")]
    [SwaggerOperation(
        OperationId = "GenerateInvoicePdf",
        Summary = "Retrieves the PDF rendering of the given invoice"
    )]
    [ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK)]
    [Produces("application/pdf")]
    public async Task<IActionResult> GenerateInvoicePdf([FromQuery] Guid invoiceId)
    {
        var userIdResult = userContextService.GetUserId();
        if (!userIdResult.IsSuccess) return Unauthorized();
        var invoice = await dbContext.Invoices
            .Include(i => i.Business)
            .Include(i => i.Items)
            .Include(i => i.Discounts)
            .FirstOrDefaultAsync(i => i.Id == invoiceId);
        if (invoice == null) return NotFound();
        if (invoice.UserId != userIdResult.Value) return Unauthorized();
        var pdfBytes = invoiceGenerationService.Generate(invoice);
        Response.Headers.Append("Content-Disposition", "inline; filename=Invoice-123.pdf");
        return File(pdfBytes, "application/pdf", $"{invoice.InvoiceNumber}.pdf");
    }

    private string GetAutoGeneratedInvoiceNumber(DateTime date, int count)
    {
        var sb = new StringBuilder();
        sb.Append("INV");
        sb.Append(date.Year);
        sb.Append(PadNumber(date.Month));
        sb.Append(PadNumber(date.Day));
        sb.Append(PadNumber(count + 1, length: 3));
        return sb.ToString();
    }

    private string PadNumber(int number, int length = 2, string pad = "0")
    {
        var result = number.ToString();
        while (result.Length < length)
        {
            result = pad + result;
        }
        return result;
    }
}