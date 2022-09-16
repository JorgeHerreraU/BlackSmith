using BlackSmith.Business.Interfaces;
using BlackSmith.Domain.Interfaces;
using BlackSmith.Domain.Models;
using FluentValidation;

namespace BlackSmith.Business.Components;

public class DoctorsBL
{
    private readonly IComplexValidator<Doctor> _complexValidator;
    private readonly IRepository<Doctor> _repository;
    private readonly IValidator<Doctor> _validator;

    public DoctorsBL(IRepository<Doctor> repository,
        IValidator<Doctor> validator,
        IComplexValidator<Doctor> complexValidator)
    {
        _repository = repository;
        _validator = validator;
        _complexValidator = complexValidator;
    }

    public async Task<IEnumerable<Doctor>> GetDoctors()
    {
        return await _repository.GetAll(doctor => doctor.Address, doctor => doctor.WorkingDays);
    }

    public async Task<Doctor> CreateDoctor(Doctor doctor)
    {
        await _validator.ValidateAndThrowAsync(doctor);
        await _complexValidator.ValidateCreateAndThrowAsync(doctor);
        await _repository.Add(doctor);
        return doctor;
    }

    public async Task<Doctor> UpdateDoctor(Doctor doctor)
    {
        await _validator.ValidateAndThrowAsync(doctor);
        await _complexValidator.ValidateUpdateAndThrowAsync(doctor);
        await _repository.Update(doctor, d => d.WorkingDays, x => x.Address);
        return doctor;
    }

    public async Task<bool> DeleteDoctor(Doctor doctor)
    {
        return await _repository.Delete(doctor);
    }
}
