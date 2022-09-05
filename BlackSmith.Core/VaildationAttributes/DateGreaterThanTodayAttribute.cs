using System.ComponentModel.DataAnnotations;

namespace BlackSmith.Core.VaildationAttributes;

public class DateGreaterThanTodayAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        return value == null
            ? new ValidationResult(ErrorMessage = "Date cannot be null")
            : (DateTime)value >= DateTime.UtcNow
                ? ValidationResult.Success
                : new ValidationResult(ErrorMessage ?? "Make sure your date is >= than today");
    }
}
