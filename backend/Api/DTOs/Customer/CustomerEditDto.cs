using Logic.Utils.Validations;

namespace Api.DTOs.Customer;

public class CustomerEditDto
{
    public Guid? Id { get; set; }
    public required string Name { get; set; }
    [ValidEmailAddress]
    public required string Email { get; set; }
    public required string Phone { get; set; }
    public required string Address { get; set; }
}
