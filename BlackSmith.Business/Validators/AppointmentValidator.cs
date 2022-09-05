using BlackSmith.Domain.Models;
using FluentValidation;

namespace BlackSmith.Business.Validators;

public class AppointmentValidator : AbstractValidator<Appointment>
{
    public AppointmentValidator()
    {
        RuleFor(x => x.Start).NotEmpty().NotNull();
        RuleFor(x => x.End).NotEmpty().NotNull().GreaterThan(x => x.Start);
        RuleFor(x => x.Doctor).NotEmpty().NotNull();
        RuleFor(x => x.Patient).NotEmpty().NotNull();
        RuleFor(x => x.DoctorId).NotEmpty().NotNull();
        RuleFor(x => x.PatientId).NotEmpty().NotNull();
    }
}
