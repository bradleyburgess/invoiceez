namespace Api.DTOs.Auth;

public class AccessTokenDto
{
    public string Token { get; set; } = null!;
    public DateTime ExpiresAtUtc { get; set; }

}
