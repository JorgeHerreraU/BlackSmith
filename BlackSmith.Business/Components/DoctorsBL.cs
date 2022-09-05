using BlackSmith.Domain.Interfaces;
using BlackSmith.Domain.Models;
using FluentValidation;

namespace BlackSmith.Business.Components;

public class DoctorsBL
{
    private readonly IRepository<Doctor> _repository;
    private readonly IValidator<Doctor> _validator;

    public DoctorsBL(IRepository<Doctor> repository,
        IValidator<Doctor> validator)
    {
        _repository = repository;
        _validator = validator;
    }

    public async Task<IEnumerable<Doctor>> GetDoctors()
    {
        return await _repository.GetAll(doctor => doctor.Address, doctor => doctor.WorkingDays);
    }

    public async Task<IEnumerable<Doctor>> GetDoctorsBySpeciality(Speciality speciality)
    {
        return await _repository.GetAll(
            doctor => doctor.Speciality == speciality,
            doctor => doctor.Address,
            x => x.WorkingDays);
    }

    public async Task<Doctor> CreateDoctor(Doctor doctor)
    {
        await _validator.ValidateAndThrowAsync(doctor);
        if (await DoctorEmailExists(doctor.Email))
            throw new ArgumentException("Doctor e-mail address is already registered");
        await _repository.Add(doctor);
        return doctor;
    }

    public async Task<Doctor> UpdateDoctor(Doctor doctor)
    {
        await _validator.ValidateAndThrowAsync(doctor);
        await _repository.Update(doctor, d => d.WorkingDays, x => x.Address);
        return doctor;
    }

    public async Task<bool> DeleteDoctor(Doctor doctor)
    {
        return await _repository.Delete(doctor);
    }

    public bool GetSpecialityIsAvailable(IEnumerable<Doctor> doctors, DateTime day)
    {
        return doctors.SelectMany(doctor => doctor.WorkingDays)
            .Select(workingDay => workingDay.Day)
            .Distinct()
            .Contains(day.DayOfWeek);
    }

    private async Task<bool> DoctorEmailExists(string email)
    {
        return await _repository.Get(doctor =>
            string.Equals(doctor.Email, email, StringComparison.CurrentCultureIgnoreCase)) is not null;
    }
}
