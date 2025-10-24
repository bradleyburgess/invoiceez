namespace Api.DTOs.Auth;

public class AuthResponseDto
{
    public required UserDto User { get; set; }
    public required TokenResponseDto Tokens { get; set; }
}
