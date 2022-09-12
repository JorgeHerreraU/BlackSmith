using BlackSmith.Business.Interfaces;
using BlackSmith.Domain.Models;

namespace BlackSmith.Business.ComplexValidators;

public class DoctorComplexValidator : IComplexValidator<Doctor>
{
    private readonly IEnumerable<ICreateComplexValidation<Doctor>> _createValidations;
    private readonly IEnumerable<IUpdateComplexValidation<Doctor>> _updateValidations;


    public DoctorComplexValidator(IEnumerable<IUpdateComplexValidation<Doctor>> updateValidations,
        IEnumerable<ICreateComplexValidation<Doctor>> createValidations)
    {
        _updateValidations = updateValidations;
        _createValidations = createValidations;
    }
    public async Task ValidateUpdateAndThrowAsync(Doctor doctor)
    {
        foreach (var validation in _updateValidations)
        {
            await validation.ValidateAsync(doctor);
        }
    }
    public async Task ValidateCreateAndThrowAsync(Doctor doctor)
    {
        foreach (var validation in _createValidations)
        {
            await validation.ValidateAsync(doctor);
        }
    }
}
