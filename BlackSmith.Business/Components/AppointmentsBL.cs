using BlackSmith.Business.Interfaces;
using BlackSmith.Core.ExtensionMethods;
using BlackSmith.Core.Structs;
using BlackSmith.Domain.Interfaces;
using BlackSmith.Domain.Models;
using FluentValidation;

namespace BlackSmith.Business.Components;

public class AppointmentsBL
{
    private readonly AppointmentsDoctorsBL _appointmentsDoctorsBL;
    private readonly IRepository<Appointment> _appointmentsRepository;
    private readonly IComplexValidator<Appointment> _complexValidator;
    private readonly IValidator<Appointment> _validator;

    public AppointmentsBL(
        IRepository<Appointment> repository,
        IValidator<Appointment> validator,
        AppointmentsDoctorsBL appointmentsDoctorsBL,
        IComplexValidator<Appointment> complexValidator)
    {
        _appointmentsRepository = repository;
        _validator = validator;
        _appointmentsDoctorsBL = appointmentsDoctorsBL;
        _complexValidator = complexValidator;
    }

    public async Task<IEnumerable<Appointment>> GetAppointments()
    {
        return await _appointmentsRepository.GetAll(a => a.Doctor, a => a.Patient);
    }

    public async IAsyncEnumerable<DateTime> GetAvailableDaysBySpeciality(Speciality speciality, DateRange dateRange)
    {
        var doctors = (await _appointmentsDoctorsBL.GetDoctorsBySpeciality(speciality)).ToList();
        var appointments = (await GetAppointmentsByDoctorsSpeciality(speciality)).ToList();
        foreach (var day in dateRange.Dates)
        {
            var isAWorkingDay = _appointmentsDoctorsBL.GetSpecialityIsAvailable(doctors, day);
            if (!isAWorkingDay) continue;
            var isDayFullyBooked = _appointmentsDoctorsBL.GetDoctorsAreFullyBookedOnSpecificDay(
                doctors,
                appointments,
                day
            );
            if (!isDayFullyBooked) yield return day;
        }
    }

    public async Task<Appointment?> GetAppointmentByPatientAndDate(Patient patient, DateTime date)
    {
        return await _appointmentsRepository.Get(a => a.PatientId == patient.Id && a.Start == date);
    }

    public async IAsyncEnumerable<DateTime> GetAvailableHoursByDoctor(Doctor doctor, DateTime date)
    {
        var appointments = await _appointmentsDoctorsBL.GetAppointmentsByDoctorAndDay(doctor, date);
        var scheduledTimes = appointments.Select(a => TimeOnly.FromDateTime(a.Start)).ToList();
        var workingRange = _appointmentsDoctorsBL.GetWorkingTimes(doctor.WorkingDays, date.DayOfWeek);
        foreach (var time in workingRange.Times)
        {
            if (!scheduledTimes.Contains(time)) yield return date.Date + time.ToTimeSpan();
        }
    }

    public async Task<Appointment> CreateAppointment(Appointment appointment)
    {
        await _validator.ValidateAndThrowAsync(appointment);
        await _complexValidator.ValidateCreateAndThrowAsync(appointment);
        appointment.SetInstancesOfTypeClassToNull();
        await _appointmentsRepository.Add(appointment);
        return appointment;
    }

    public async Task<Appointment> UpdateAppointment(Appointment appointment)
    {
        await _validator.ValidateAndThrowAsync(appointment);
        await _complexValidator.ValidateUpdateAndThrowAsync(appointment);
        appointment.SetInstancesOfTypeClassToNull();
        return await _appointmentsRepository.Update(appointment);
    }

    public async Task<bool> DeleteAppointment(Appointment appointment)
    {
        appointment.SetInstancesOfTypeClassToNull();
        return await _appointmentsRepository.Delete(appointment);
    }

    private async Task<IEnumerable<Appointment>> GetAppointmentsByDoctorsSpeciality(Speciality speciality)
    {
        return await _appointmentsRepository.GetAll(a => a.Doctor.Speciality == speciality,
            a => a.Doctor,
            a => a.Patient);
    }
}
