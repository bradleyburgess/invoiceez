using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Api.DTOs.Auth;
using Logic.Database;
using Logic.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Api.Services;

public class TokenService : ITokenService
{
    private readonly AppDbContext _dbContext;
    private readonly IConfiguration _configuration;
    private readonly ILogger<TokenService> _logger;
    private readonly IHashingService _hashingService;
    private readonly SymmetricSecurityKey _key;

    private const string AllowedChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

    public TokenService(
        AppDbContext dbContext,
        IConfiguration configuration,
        ILogger<TokenService> logger,
        IHashingService hashingService
    )
    {
        _dbContext = dbContext;
        _configuration = configuration;
        _logger = logger;
        _hashingService = hashingService;
        _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]!));

    }

    public AccessTokenDto GenerateAccessToken(User user)
    {
        var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new(ClaimTypes.Email, user.Email!),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

        var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.Now.AddMinutes(15),
            SigningCredentials = creds,
            Issuer = _configuration["JWT:Issuer"],
            Audience = _configuration["JWT:Audience"]
        };

        var tokenHandler = new JwtSecurityTokenHandler();

        var token = tokenHandler.CreateToken(tokenDescriptor);

        var tokenString = tokenHandler.WriteToken(token);
        return new AccessTokenDto
        {
            Token = tokenString,
            ExpiresAtUtc = tokenDescriptor.Expires!.Value.ToUniversalTime()
        };
    }

    public async Task<RefreshTokenDto> CreateRefreshToken(User user)
    {
        var rawToken = GenerateAlphanumericToken(64);
        var hashedToken = _hashingService.Hash(rawToken);
        var refreshToken = new RefreshToken
        {
            HashedToken = hashedToken,
            UserId = user.Id,
            ExpiresAtUtc = DateTime.UtcNow.AddDays(7)
        };
        _dbContext.RefreshTokens.Add(refreshToken);
        await _dbContext.SaveChangesAsync();
        return new RefreshTokenDto
        {
            Token = rawToken,
            UserId = user.Id,
            ExpiresAtUtc = refreshToken.ExpiresAtUtc
        };
    }

    public async Task<RefreshToken?> GetRefreshTokenAsync(string token)
    {
        var hashedToken = _hashingService.Hash(token);
        return await _dbContext.RefreshTokens
            .Where(rt => !rt.IsRevoked && rt.ExpiresAtUtc > DateTime.UtcNow)
            .FirstOrDefaultAsync(rt => rt.HashedToken == hashedToken);
    }

    public async Task RevokeRefreshTokenAsync(RefreshToken token)
    {
        token.IsRevoked = true;
        _dbContext.RefreshTokens.Update(token);
        await _dbContext.SaveChangesAsync();
    }

    private static string GenerateAlphanumericToken(int length = 64)
    {
        var bytes = new byte[length];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(bytes);

        var result = new StringBuilder(length);
        foreach (var b in bytes)
        {
            result.Append(AllowedChars[b % AllowedChars.Length]);
        }

        return result.ToString();
    }
}

public interface ITokenService
{
    AccessTokenDto GenerateAccessToken(User user);
    Task<RefreshTokenDto> CreateRefreshToken(User user);
    Task<RefreshToken?> GetRefreshTokenAsync(string token);
    Task RevokeRefreshTokenAsync(RefreshToken token);
}
