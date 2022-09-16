using BlackSmith.Business.Interfaces;
using BlackSmith.Domain.Interfaces;
using BlackSmith.Domain.Models;

namespace BlackSmith.Business.ComplexValidators;

public class PatientExistsValidation : ICreateComplexValidation<Patient>
{
    private readonly IRepository<Patient> _repository;

    public PatientExistsValidation(IRepository<Patient> repository)
    {
        _repository = repository;
    }

    public async Task ValidateAsync(Patient patient)
    {
        if (await PatientEmailExists(patient.Email))
            throw new ArgumentException("Patient e-mail address is already registered");
    }

    private async Task<bool> PatientEmailExists(string email)
    {
        return await _repository.Get(p => string.Equals(p.Email, email)) is not null;
    }
}
