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
    private readonly IValidator<Appointment> _validator;

    public AppointmentsBL(
        IRepository<Appointment> repository,
        IValidator<Appointment> validator,
        AppointmentsDoctorsBL appointmentsDoctorsBL)
    {
        _appointmentsRepository = repository;
        _validator = validator;
        _appointmentsDoctorsBL = appointmentsDoctorsBL;
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
            var isDayFullyBooked = GetDoctorsAreFullyBookedOnSpecificDay(
                doctors,
                appointments,
                day
            );
            if (!isDayFullyBooked) yield return day;
        }
    }

    private async Task<bool> GetDoctorIsFullyBookedOnSpecificDate(Doctor doctor, DateTime date)
    {
        var appointments = await GetAppointmentsByDoctorAndDay(doctor, date);
        return GetWorkingDayIsFullyBooked(doctor.WorkingDays.ToList(), date, appointments);
    }

    public async Task<Appointment?> GetAppointmentByPatientAndDate(Patient patient, DateTime date)
    {
        return await _appointmentsRepository.Get(a => a.PatientId == patient.Id && a.Start == date);
    }

    public async IAsyncEnumerable<DateTime> GetAvailableHoursByDoctor(Doctor doctor, DateTime date)
    {
        var appointments = await GetAppointmentsByDoctorAndDay(doctor, date);
        var scheduledTimes = appointments.Select(a => TimeOnly.FromDateTime(a.Start)).ToList();
        var workingRange = _appointmentsDoctorsBL.GetWorkingTimes(doctor.WorkingDays, date.DayOfWeek);
        foreach (var time in workingRange.Times)
        {
            if (!scheduledTimes.Contains(time)) yield return date.Date + time.ToTimeSpan();
        }
    }

    private async Task<IEnumerable<Appointment>> GetAppointmentsByDoctorsSpeciality(
        Speciality speciality
    )
    {
        return await _appointmentsRepository.GetAll(a => a.Doctor.Speciality == speciality,
            a => a.Doctor,
            a => a.Patient);
    }

    public async Task<Appointment> CreateAppointment(Appointment appointment)
    {
        await _validator.ValidateAndThrowAsync(appointment);
        var isDoctorFullyBooked = await GetDoctorIsFullyBookedOnSpecificDate(
            appointment.Doctor,
            appointment.Start
        );
        if (isDoctorFullyBooked) throw new ArgumentException("Doctor is fully booked on this day");
        appointment.ClearClassInstances();
        await _appointmentsRepository.Add(appointment);
        return appointment;
    }

    public async Task<Appointment> UpdateAppointment(Appointment appointment)
    {
        await _validator.ValidateAndThrowAsync(appointment);
        var isDoctorFullyBooked = await GetDoctorIsFullyBookedOnSpecificDate(
            appointment.Doctor,
            appointment.Start
        );
        if (isDoctorFullyBooked) throw new ArgumentException("Doctor is fully booked on this day");
        appointment.ClearClassInstances();
        return await _appointmentsRepository.Update(appointment);
    }

    public async Task<bool> DeleteAppointment(Appointment appointment)
    {
        appointment.ClearClassInstances();
        return await _appointmentsRepository.Delete(appointment);
    }

    private async Task<IEnumerable<Appointment>> GetAppointmentsByDoctorAndDay(Doctor doctor, DateTime date)
    {
        return await _appointmentsRepository.GetAll(a => a.Doctor == doctor && a.Start.DayOfWeek == date.DayOfWeek,
            a => a.Doctor,
            a => a.Patient);
    }

    private bool GetDoctorsAreFullyBookedOnSpecificDay(
        IEnumerable<Doctor> doctors,
        IEnumerable<Appointment> appointments,
        DateTime date
    )
    {
        var workingDays = doctors.SelectMany(d => d.WorkingDays).ToList();
        return GetWorkingDayIsFullyBooked(workingDays, date, appointments);
    }

    private bool GetWorkingDayIsFullyBooked(
        ICollection<WorkingDay> workingDays,
        DateTime date,
        IEnumerable<Appointment> appointments
    )
    {
        var workingRange = _appointmentsDoctorsBL.GetWorkingTimes(workingDays, date.DayOfWeek);
        var scheduledWorkTimeRange = appointments
            .Where(a => a.Start.Date == date.Date)
            .Select(a => TimeOnly.FromDateTime(a.Start)).ToList();
        return workingRange.Times.ToList().HasAll(scheduledWorkTimeRange);
    }
}
