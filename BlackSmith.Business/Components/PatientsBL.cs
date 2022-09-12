using BlackSmith.Business.Interfaces;
using BlackSmith.Domain.Interfaces;
using BlackSmith.Domain.Models;
using FluentValidation;

namespace BlackSmith.Business.Components;

public class PatientsBL
{
    private readonly IComplexValidator<Patient> _complexValidator;
    private readonly IRepository<Patient> _repository;
    private readonly IValidator<Patient> _validator;

    public PatientsBL(IRepository<Patient> repository,
        IValidator<Patient> validator,
        IComplexValidator<Patient> complexValidator)
    {
        _repository = repository;
        _validator = validator;
        _complexValidator = complexValidator;
    }

    public async Task<IEnumerable<Patient>> GetPatients()
    {
        return await _repository.GetAll(p => p.Address);
    }

    public async Task<Patient> CreatePatient(Patient patient)
    {
        await _validator.ValidateAndThrowAsync(patient);
        await _complexValidator.ValidateCreateAndThrowAsync(patient);
        await _repository.Add(patient);
        return patient;
    }

    public async Task<Patient> UpdatePatient(Patient patient)
    {
        await _validator.ValidateAndThrowAsync(patient);
        await _repository.Update(patient);
        return patient;
    }

    public async Task<bool> DeletePatient(Patient patient)
    {
        return await _repository.Delete(patient);
    }
}
