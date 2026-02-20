using Api.DTOs;
using Api.DTOs.Business;
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
public class BusinessesController(
    AppDbContext dbContext,
    IUserContextService userContextService,
    ILogger<BusinessesController> logger
) : ControllerBase
{
    [HttpGet]
    [SwaggerOperation(OperationId = "GetBusinesses", Summary = "Get all businesses for the authenticated user")]
    public async Task<ActionResult<ApiResponse<BusinessesResponseDto>>> GetBusinesses()
    {
        var userResult = await userContextService.GetUserAsync();
        if (!userResult.IsSuccess)
        {
            return NotFound(ApiResponse<BusinessesResponseDto>.Fail(
                ApiResponseCode.NotFound,
                userResult.ErrorMessage
            ));
        }
        var user = userResult.Value!;

        var businesses = await dbContext.Businesses
            .Where(b => b.UserId == user.Id)
            .Select(b => b.MapToDto())
            .ToListAsync();
        return Ok(ApiResponse<BusinessesResponseDto>.Ok(
            new BusinessesResponseDto
            {
                Businesses = businesses
            }
        ));
    }

    [HttpGet("{id:guid}")]
    [SwaggerOperation(OperationId = "GetBusiness", Summary = "Get a business by ID for the authenticated user")]
    public async Task<ActionResult<ApiResponse<BusinessDto>>> GetBusiness(Guid id)
    {
        var userIdResult = userContextService.GetUserId();
        var business = await dbContext.Businesses
            .Where(b => b.Id == id && b.UserId == userIdResult.Value)
            .Select(b => b.MapToDto())
            .FirstOrDefaultAsync();
        if (business == null)
        {
            return NotFound(ApiResponse<BusinessDto>.Fail(
                ApiResponseCode.NotFound,
                "Business not found"
            ));
        }
        return Ok(ApiResponse<BusinessDto>.Ok(business));
    }

    [HttpPost]
    [SwaggerOperation(OperationId = "CreateBusiness", Summary = "Create a new business for the authenticated user")]
    public async Task<ActionResult<ApiResponse<BusinessDto>>> CreateBusiness(BusinessEditDto dto)
    {
        var userResult = await userContextService.GetUserAsync();
        if (!userResult.IsSuccess || userResult.Value == null)
        {
            return NotFound(ApiResponse<BusinessDto>.Fail(
                ApiResponseCode.NotFound,
                "User not found"
            ));
        }
        var business = dto.MapToEntity(userResult.Value.Id);
        dbContext.Businesses.Add(business);
        await dbContext.SaveChangesAsync();
        var businessDto = business.MapToDto();
        logger.LogInformation("Business {BusinessId} created for user {UserId}", business.Id, userResult.Value.Id);
        return CreatedAtAction(
            nameof(GetBusiness),
            new { id = business.Id },
            ApiResponse<BusinessDto>.Ok(businessDto)
        );
    }

    [HttpDelete("{id:guid}")]
    [SwaggerOperation(OperationId = "DeleteBusiness", Summary = "Delete a business by ID for the authenticated user")]
    public async Task<ActionResult<ApiResponse<EmptyDto>>> DeleteBusiness(Guid id)
    {
        var userIdResult = userContextService.GetUserId();
        var business = await dbContext.Businesses
            .Where(b => b.Id == id && b.UserId == userIdResult.Value)
            .FirstOrDefaultAsync();
        if (business == null)
        {
            return NotFound(ApiResponse<EmptyDto>.Fail(
                ApiResponseCode.NotFound,
                "Business not found"
            ));
        }
        dbContext.Businesses.Remove(business);
        await dbContext.SaveChangesAsync();
        logger.LogInformation("Business {BusinessId} deleted", id);
        return Ok(ApiResponse<EmptyDto>.Ok(message: "Business deleted successfully"));
    }

    [HttpPut("{id:guid}")]
    [SwaggerOperation(OperationId = "UpdateBusiness", Summary = "Update a business by ID for the authenticated user")]
    public async Task<ActionResult<ApiResponse<BusinessDto>>> UpdateBusiness(Guid id, BusinessEditDto dto)
    {
        var userIdResult = userContextService.GetUserId();
        var business = await dbContext.Businesses
            .Where(b => b.Id == id && b.UserId == userIdResult.Value)
            .FirstOrDefaultAsync();
        if (business == null)
        {
            return NotFound(ApiResponse<BusinessDto>.Fail(
                ApiResponseCode.NotFound,
                "Business not found"
            ));
        }
        business.UpdateFromDto(dto);
        await dbContext.SaveChangesAsync();
        var businessDto = business.MapToDto();
        logger.LogInformation("Business {BusinessId} updated", id);
        return Ok(ApiResponse<BusinessDto>.Ok(businessDto, "Business updated successfully"));
    }
}
