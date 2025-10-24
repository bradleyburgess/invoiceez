using Api.DTOs;
using Api.DTOs.Account;
using Api.DTOs.Auth;
using Api.Extensions;
using Api.Extensions.Mapping;
using Api.Services;
using Logic.Database;
using Logic.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AccountController(
    AppDbContext dbContext,
    IUserContextService userContextService,
    UserManager<User> userManager
) : ControllerBase
{
    [HttpGet]
    [SwaggerOperation(OperationId = "GetAccountInfo", Summary = "Get current user's account information")]
    public async Task<ActionResult<ApiResponse<UserDto>>> GetAccountInfo()
    {
        var userResult = await userContextService.GetUserAsync();
        if (!userResult.IsSuccess || userResult.Value == null)
        {
            return NotFound(ApiResponse<UserDto>.Fail(
                ApiResponseCode.NotFound,
                "User not found"
            ));
        }

        var user = userResult.Value!;
        return Ok(ApiResponse<UserDto>.Ok(user.MapToDto()));
    }

    [HttpPut]
    [SwaggerOperation(OperationId = "UpdateAccountInfo", Summary = "Update current user's account information")]
    public async Task<ActionResult<ApiResponse<UserDto>>> UpdateAccountInfo(UserEditDto dto)
    {
        var userResult = await userContextService.GetUserAsync();
        if (!userResult.IsSuccess || userResult.Value == null)
        {
            return NotFound(ApiResponse<UserDto>.Fail(
                 ApiResponseCode.NotFound,
                 "User not found"
            ));
        }

        var user = userResult.Value!;

        user.Email = dto.Email ?? user.Email;
        user.FirstName = dto.FirstName ?? user.FirstName;
        user.LastName = dto.LastName ?? user.LastName;
        await userManager.SetUserNameAsync(user, dto.Email ?? user.UserName);

        dbContext.Users.Update(user);
        await dbContext.SaveChangesAsync();

        return Ok(ApiResponse<UserDto>.Ok(user.MapToDto()));
    }

    [HttpPut("change-password")]
    [SwaggerOperation(OperationId = "ChangePassword", Summary = "Change current user's password")]
    public async Task<ActionResult<ApiResponse<object>>> ChangePassword(UserChangePasswordRequestDto dto)
    {
        var userResult = await userContextService.GetUserAsync();
        if (!userResult.IsSuccess || userResult.Value == null)
        {
            return NotFound(ApiResponse<object>.Fail(ApiResponseCode.NotFound, "User not found"));
        }

        var user = userResult.Value!;

        var changePasswordResult = await userManager.ChangePasswordAsync(user, dto.CurrentPassword, dto.NewPassword);
        if (!changePasswordResult.Succeeded)
        {
            var errors = new Dictionary<string, string[]>();
            if (changePasswordResult.Errors.Any(e => e.Code == "PasswordMismatch"))
            {
                errors.Add("CurrentPassword", new[] { "The current password is incorrect." });
            }
            else
            {
                foreach (var error in changePasswordResult.Errors)
                {
                    if (errors.ContainsKey("NewPassword"))
                    {
                        var existingErrors = errors["NewPassword"].ToList();
                        existingErrors.Add(error.Description);
                        errors["NewPassword"] = existingErrors.ToArray();
                    }
                    else
                    {
                        errors.Add("NewPassword", new[] { error.Description });
                    }
                }
            }
            return ApiResponse<object>.Fail(
                ApiResponseCode.ValidationError,
                null,
                errors
            );
        }

        return Ok(ApiResponse<object>.Ok("Password changed successfully"));
    }
}
