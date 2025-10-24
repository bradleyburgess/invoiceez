using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Logic.Models;

public class User : IdentityUser<Guid>
{
    public string FirstName { get; set; } = String.Empty;
    public string LastName { get; set; } = String.Empty;

    public ICollection<Business> Businesses { get; set; } = [];
    public ICollection<RefreshToken> RefreshTokens { get; set; } = [];
}
