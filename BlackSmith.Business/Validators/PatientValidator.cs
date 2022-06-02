using BlackSmith.Domain.Models;
using FluentValidation;

namespace BlackSmith.Business.Validators;

public class PatientValidator : AbstractValidator<Patient>
{
    public PatientValidator()
    {
        RuleFor(x => x.FirstName).NotNull();
    }
}