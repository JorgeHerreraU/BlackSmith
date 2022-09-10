using BlackSmith.Core.ExtensionMethods;
using BlackSmith.Domain.Interfaces;
using BlackSmith.Domain.Models;
using FluentValidation;

namespace BlackSmith.Business.Components;

public class DoctorsBL
{
    private readonly AppointmentsDoctorsBL _appointmentsDoctorsBL;
    private readonly IRepository<Doctor> _repository;
    private readonly IValidator<Doctor> _validator;

    public DoctorsBL(IRepository<Doctor> repository,
        IValidator<Doctor> validator,
        AppointmentsDoctorsBL appointmentsDoctorsBL)
    {
        _repository = repository;
        _validator = validator;
        _appointmentsDoctorsBL = appointmentsDoctorsBL;
    }

    public async Task<IEnumerable<Doctor>> GetDoctors()
    {
        return await _repository.GetAll(doctor => doctor.Address, doctor => doctor.WorkingDays);
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
        // TODO: Check for existing appointments before updating
        var appointments = (await _appointmentsDoctorsBL.GetUpcomingAppointmentsByDoctor(doctor)).ToList();
        if (HasMissingWorkingDay(appointments, doctor))
            throw new ArgumentException("Doctor has one or more appointments, please review the agenda");
        if (HasMissingWorkingHour(appointments, doctor))
            throw new ArgumentException("Doctor has one or more appointments, please review the agenda");
        await _repository.Update(doctor, d => d.WorkingDays, x => x.Address);
        return doctor;
    }
    private bool HasMissingWorkingHour(IEnumerable<Appointment> appointments, Doctor doctor)
    {
        var appointmentDates = appointments.Select(x => x.Start).ToList();
        var incomingWorkingDays = doctor.WorkingDays;
        var matchingDays = appointmentDates.Where(d => incomingWorkingDays.Any(w => w.Day == d.DayOfWeek));
        return (from scheduled in matchingDays
            let workingRange = _appointmentsDoctorsBL.GetWorkingTimes(incomingWorkingDays, scheduled.DayOfWeek)
            where scheduled.Hour <= workingRange.Start.Hour || scheduled.Hour >= workingRange.End.Hour
            select scheduled).Any();
    }

    public async Task<bool> DeleteDoctor(Doctor doctor)
    {
        // TODO: Check for existing appointments before removing
        return await _repository.Delete(doctor);
    }

    private async Task<bool> DoctorEmailExists(string email)
    {
        return await _repository.Get(d => string.Equals(d.Email, email)) is not null;
    }

    private bool HasMissingWorkingDay(IEnumerable<Appointment> appointments, Doctor doctor)
    {
        var appointmentsDayOfWeek = appointments.Select(x => x.Start.DayOfWeek).Distinct().ToList();
        var incomingDayOfWeek = doctor.WorkingDays.Select(x => x.Day).ToList();
        return appointmentsDayOfWeek.HasNotAll(incomingDayOfWeek);
    }
}
