namespace Logic.Models;

public class RefreshToken
{
    public int Id { get; set; }
    public string HashedToken { get; set; } = string.Empty;
    public DateTime ExpiresAtUtc { get; set; }
    public bool IsRevoked { get; set; } = false;

    public Guid UserId { get; set; }
    public User? User { get; set; }
}
