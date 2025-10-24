namespace Api.DTOs.Auth;

public class RefreshTokenDto
{
    public string Token { get; set; } = string.Empty;
    public Guid UserId { get; set; }
    public DateTime ExpiresAtUtc { get; set; }
}
