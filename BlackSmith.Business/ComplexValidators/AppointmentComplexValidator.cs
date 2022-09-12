using BlackSmith.Business.Interfaces;
using BlackSmith.Domain.Models;

namespace BlackSmith.Business.ComplexValidators;

public class AppointmentComplexValidator : IComplexValidator<Appointment>
{
    private readonly IEnumerable<ICreateComplexValidation<Appointment>> _createValidations;
    private readonly IEnumerable<IUpdateComplexValidation<Appointment>> _updateValidations;

    public AppointmentComplexValidator(IEnumerable<ICreateComplexValidation<Appointment>> createValidations,
        IEnumerable<IUpdateComplexValidation<Appointment>> updateValidations)
    {
        _createValidations = createValidations;
        _updateValidations = updateValidations;
    }
    public async Task ValidateUpdateAndThrowAsync(Appointment appointment)
    {
        foreach (var validation in _updateValidations)
        {
            await validation.ValidateAsync(appointment);
        }
    }
    public async Task ValidateCreateAndThrowAsync(Appointment appointment)
    {
        foreach (var validation in _createValidations)
        {
            await validation.ValidateAsync(appointment);
        }
    }
}
