using BlackSmith.Business.Interfaces;
using BlackSmith.Domain.Models;

namespace BlackSmith.Business.ComplexValidators;

public class PatientComplexValidator : IComplexValidator<Patient>
{
    private readonly IEnumerable<ICreateComplexValidation<Patient>> _createValidations;

    public PatientComplexValidator(IEnumerable<ICreateComplexValidation<Patient>> createValidations)
    {
        _createValidations = createValidations;
    }
    public Task ValidateUpdateAndThrowAsync(Patient t)
    {
        return null!;
    }
    public async Task ValidateCreateAndThrowAsync(Patient patient)
    {
        foreach (var validation in _createValidations)
        {
            await validation.ValidateAsync(patient);
        }
    }
}
