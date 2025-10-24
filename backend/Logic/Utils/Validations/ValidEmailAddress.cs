using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Logic.Utils.Validations;

public class ValidEmailAddress : ValidationAttribute
{
    private readonly string emailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
            return ValidationResult.Success;

        var email = value.ToString();

        if (Regex.IsMatch(email!, emailPattern))
            return ValidationResult.Success;

        return new ValidationResult(ErrorMessage ?? "Invalid email address format.");
    }

}
