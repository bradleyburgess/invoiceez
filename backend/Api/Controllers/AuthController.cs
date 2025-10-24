using Api.Configuration;
using Api.DTOs;
using Api.DTOs.Auth;
using Api.Extensions.Mapping;
using Api.Services;
using Logic.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(
    UserManager<User> userManager,
    SignInManager<User> signInManager,
    ITokenService tokenService,
    IOptions<AppSettings> options
) : ControllerBase
{
    [HttpPost("register")]
    [SwaggerOperation(OperationId = "Register", Summary = "Register a new user")]
    public async Task<ActionResult<ApiResponse<AuthResponseDto>>> Register(AuthRegisterRequestDto dto)
    {
        var existingUser = await userManager.FindByEmailAsync(dto.Email);
        if (existingUser != null)
        {
            return BadRequest(ApiResponse<AuthResponseDto>.Fail(
                ApiResponseCode.BadRequest,
                "Email is already in use"
            ));
        }
        var user = dto.MapToEntity();
        var result = await userManager.CreateAsync(user, dto.Password);

        if (result.Succeeded)
        {
            return await LoginUser(user, "User registration successful!");
        }

        return BadRequest(ApiResponse<AuthResponseDto>.Fail(
            ApiResponseCode.ServerError,
            String.Join(", ", result.Errors.Select(x => x.Description))
        ));
    }

    [HttpPost("login")]
    [SwaggerOperation(OperationId = "Login", Summary = "Login a user")]
    public async Task<ActionResult<ApiResponse<AuthResponseDto>>> Login(AuthLoginRequestDto dto)
    {
        var result = await signInManager.PasswordSignInAsync(dto.Email, dto.Password, false, false);
        if (!result.Succeeded)
        {
            return Unauthorized(ApiResponse<AuthResponseDto>.Fail(
                ApiResponseCode.Unauthorized,
                "Invalid email or password"
            ));
        }
        return await LoginUser(await userManager.FindByEmailAsync(dto.Email));
    }

    [HttpPost("logout")]
    [SwaggerOperation(OperationId = "Logout", Summary = "Logout the current user")]
    public async Task<IActionResult> Logout()
    {
        await signInManager.SignOutAsync();
        DeleteRefreshTokenCookie();
        return Ok(ApiResponse<EmptyDto>.Ok(message: "Logged out successfully"));
    }

    [HttpPost("refresh-token")]
    [SwaggerOperation(OperationId = "RefreshToken", Summary = "Refresh access token using refresh token")]
    public async Task<ActionResult<ApiResponse<AuthResponseDto>>> RefreshToken(AuthRefreshRequestDto dto)
    {
        var refreshToken = Request.Cookies.ContainsKey("RefreshToken")
            ? Request.Cookies["RefreshToken"]
            : dto.RefreshToken;
        if (string.IsNullOrEmpty(refreshToken))
        {
            return BadRequest(ApiResponse<TokenResponseDto>.Fail(
                ApiResponseCode.BadRequest,
                "Refresh token is required"
            ));
        }
        var existingToken = await tokenService.GetRefreshTokenAsync(refreshToken);
        if (existingToken == null)
        {
            return Unauthorized(ApiResponse<TokenResponseDto>.Fail(
                ApiResponseCode.Unauthorized,
                "Invalid or expired refresh token"
            ));
        }

        var user = await userManager.FindByIdAsync(existingToken.UserId.ToString());
        if (user == null)
        {
            return Unauthorized(ApiResponse<TokenResponseDto>.Fail(
                ApiResponseCode.NotFound,
                "User not found"
            ));
        }

        var accessToken = tokenService.GenerateAccessToken(user);

        await tokenService.RevokeRefreshTokenAsync(existingToken);
        var newRefreshToken = await tokenService.CreateRefreshToken(user);

        var tokenResponse = new TokenResponseDto
        {
            AccessToken = accessToken.Token,
            RefreshToken = newRefreshToken.Token,
            ExpiresAtUtc = accessToken.ExpiresAtUtc,
        };
        var userResponse = user.MapToDto();
        var response = new AuthResponseDto
        {
            User = userResponse,
            Tokens = tokenResponse
        };

        SetRefreshTokenCookie(newRefreshToken.Token, newRefreshToken.ExpiresAtUtc);
        return Ok(ApiResponse<AuthResponseDto>.Ok(data: response, message: "Token refreshed successfully"));
    }

    [HttpGet("check-registration-accepted")]
    [SwaggerOperation(OperationId = "CheckRegistrationAccepted", Summary = "Check if registrations are being accepted")]
    public async Task<bool> CheckRegistrationAccepted() => options.Value.AllowRegistration;

    private async Task<ActionResult<ApiResponse<AuthResponseDto>>> LoginUser(User? user, String message = "Login successful")
    {
        if (user == null)
        {
            return Unauthorized(ApiResponse<AuthResponseDto>.Fail(
                ApiResponseCode.Unauthorized,
                "Invalid email or password"
            ));
        }
        var accessToken = tokenService.GenerateAccessToken(user);
        var refreshToken = await tokenService.CreateRefreshToken(user);

        var response = new AuthResponseDto
        {
            User = user.MapToDto(),
            Tokens = new TokenResponseDto
            {
                AccessToken = accessToken.Token,
                RefreshToken = refreshToken.Token,
                ExpiresAtUtc = accessToken.ExpiresAtUtc,
            }
        };
        SetRefreshTokenCookie(refreshToken.Token, refreshToken.ExpiresAtUtc);
        return Ok(ApiResponse<AuthResponseDto>.Ok(data: response, message: message));
    }

    private void SetRefreshTokenCookie(string refreshToken, DateTime expiresAt)
    {
        HttpContext.Response.Cookies.Append("RefreshToken", refreshToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Lax,
            Expires = expiresAt
        });
    }

    private void DeleteRefreshTokenCookie()
    {
        if (Request.Cookies.ContainsKey("RefreshToken"))
        {
            HttpContext.Response.Cookies.Delete("RefreshToken");
        }
    }
}
