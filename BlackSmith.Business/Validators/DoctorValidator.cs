using BlackSmith.Domain.Models;
using FluentValidation;

namespace BlackSmith.Business.Validators;

public class DoctorValidator : AbstractValidator<Doctor>
{
    public DoctorValidator()
    {
        RuleFor(x => x.FirstName).NotEmpty().NotNull().BeAValidName();
        RuleFor(x => x.LastName).NotEmpty().NotNull().BeAValidName();
        RuleForEach(x => x.WorkingDays).SetValidator(new WorkingDayValidator());
    }
}
