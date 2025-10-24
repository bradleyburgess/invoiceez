using System.ComponentModel.DataAnnotations;

namespace Api.DTOs.Auth;

public class AuthLoginRequestDto
{
    [Required]
    [EmailAddress(ErrorMessage = "Invalid email address")]
    public string Email { get; set; } = string.Empty;

    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;
}