using BlackSmith.Domain.Interfaces;
using BlackSmith.Domain.Models;
using FluentValidation;

namespace BlackSmith.Business.Components;

public class PatientsBL
{
    private readonly IRepository<Patient> _repository;
    private readonly IValidator<Patient> _validator;

    public PatientsBL(IRepository<Patient> repository, IValidator<Patient> validator)
    {
        _repository = repository;
        _validator = validator;
    }

    public async Task<IEnumerable<Patient>> GetPatients()
    {
        return await _repository.GetAll(patient => patient.Address);
    }

    public async Task<Patient> CreatePatient(Patient patient)
    {
        await _validator.ValidateAndThrowAsync(patient);
        if (await PatientEmailExists(patient.Email))
            throw new ArgumentException("Patient e-mail address is already registered");
        await _repository.Add(patient);
        return patient;
    }

    private async Task<bool> PatientEmailExists(string email)
    {
        return await _repository.Get(patient => patient.Email.ToLower() == email.ToLower()) is not null;
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