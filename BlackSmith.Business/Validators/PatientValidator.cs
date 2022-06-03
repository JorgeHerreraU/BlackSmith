using System.Text.RegularExpressions;
using BlackSmith.Domain.Models;
using FluentValidation;

namespace BlackSmith.Business.Validators;

public class PatientValidator : AbstractValidator<Patient>
{
    public PatientValidator()
    {
        RuleFor(x => x.FirstName).NotNull().NotEmpty();
        RuleFor(x => x.LastName).NotNull().NotEmpty();
        RuleFor(x => x.LastName).NotEqual(x => x.FirstName);
        RuleFor(x => x.PhoneNumber)
            .NotNull()
            .NotEmpty()
            .MinimumLength(9)
            .MaximumLength(14)
            .Matches(new Regex(@"((\(\d{3}\) ?)|(\d{3}-))?\d{3}-\d{4}"));
        RuleFor(x => x.Email).EmailAddress();
    }
}