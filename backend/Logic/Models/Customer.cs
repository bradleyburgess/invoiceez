using Logic.Utils.Validations;

namespace Logic.Models;

public class Customer
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    [ValidEmailAddress]
    public required string Email { get; set; }
    public required string Phone { get; set; }
    public required string Address { get; set; }
    public Guid UserId { get; set; }
    public User? User { get; set; }
}
