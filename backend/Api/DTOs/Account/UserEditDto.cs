using System.ComponentModel.DataAnnotations;

namespace Api.DTOs.Account;

public class UserEditDto
{
    [Required]
    public required string FirstName { get; set; }
    [Required]
    public required string LastName { get; set; }
    [Required]
    [EmailAddress]
    public required string Email { get; set; }
}
