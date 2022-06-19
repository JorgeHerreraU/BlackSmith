using BlackSmith.Domain.Interfaces;
using BlackSmith.Domain.Models;
using FluentValidation;

namespace BlackSmith.Business.Components;

public class DoctorsBL
{
    private readonly IRepository<Doctor> _repository;
    private readonly IValidator<Doctor> _validator;

    public DoctorsBL(IRepository<Doctor> repository, IValidator<Doctor> validator)
    {
        _repository = repository;
        _validator = validator;
    }

    public async Task<IEnumerable<Doctor>> GetDoctors()
    {
        return await _repository.GetAll(x => x.Address, x => x.WorkingDays);
    }

    public async Task<Doctor> CreateDoctor(Doctor doctor)
    {
        await _validator.ValidateAndThrowAsync(doctor);
        if (await DoctorEmailExists(doctor.Email))
            throw new ArgumentException("Doctor e-mail address is already registered");
        await _repository.Add(doctor);
        return doctor;
    }

    private async Task<bool> DoctorEmailExists(string email)
    {
        return await _repository.Get(x => x.Email.ToLower() == email.ToLower()) is not null;
    }
}