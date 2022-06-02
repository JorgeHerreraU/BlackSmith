using BlackSmith.Domain.Interfaces;
using BlackSmith.Domain.Models;

namespace BlackSmith.Business.Components;

public class PatientsBL
{
    private readonly IRepository<Patient> _repository;

    public PatientsBL(IRepository<Patient> repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<Patient>> GetPatients()
    {
        return await _repository.GetAll();
    }

    public async Task<Patient> CreatePatient(Patient patient)
    {
        if (await PatientExists(patient.Id)) throw new ArgumentException("Patient already exists");
        await _repository.Add(patient);
        return patient;
    }

    private async Task<bool> PatientExists(int id)
    {
        return await _repository.GetById(id) is not null;
    }
}