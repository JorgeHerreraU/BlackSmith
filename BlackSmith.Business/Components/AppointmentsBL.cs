using BlackSmith.Core.ExtensionMethods;
using BlackSmith.Core.Structs;
using BlackSmith.Domain.Interfaces;
using BlackSmith.Domain.Models;
using FluentValidation;

namespace BlackSmith.Business.Components;

public class AppointmentsBL
{
    private readonly IRepository<Appointment> _appointmentsRepository;
    private readonly DoctorsBL _doctorsBL;
    private readonly IValidator<Appointment> _validator;

    public AppointmentsBL(
        IRepository<Appointment> repository,
        DoctorsBL doctorsBL,
        IValidator<Appointment> validator
    )
    {
        _appointmentsRepository = repository;
        _doctorsBL = doctorsBL;
        _validator = validator;
    }

    public async Task<IEnumerable<Appointment>> GetAppointments()
    {
        return await _appointmentsRepository.GetAll(a => a.Doctor, a => a.Patient);
    }

    public async IAsyncEnumerable<DateTime> GetAvailableDaysBySpeciality(Speciality speciality, DateRange dateRange)
    {
        var doctors = (await _doctorsBL.GetDoctorsBySpeciality(speciality)).ToList();
        var appointments = (await GetAppointmentsByDoctorsSpeciality(speciality)).ToList();
        foreach (var day in dateRange.Dates)
        {
            var isAWorkingDay = _doctorsBL.GetSpecialityIsAvailable(doctors, day);
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
        var workingRange = GetWorkingTimes(doctor.WorkingDays, date);
        foreach (var time in workingRange.Times)
        {
            if (!scheduledTimes.Contains(time)) yield return date.Date + time.ToTimeSpan();
        }
    }
    private static TimeRange GetWorkingTimes(ICollection<WorkingDay> workingDays, DateTime date)
    {
        var workStartTime = workingDays.Where(w => w.Day == date.DayOfWeek).Min(w => w.StartTime);
        var workFinishTime = workingDays.Where(w => w.Day == date.DayOfWeek).Max(w => w.EndTime);
        return new TimeRange(workStartTime, workFinishTime);
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

    private static bool GetDoctorsAreFullyBookedOnSpecificDay(
        IEnumerable<Doctor> doctors,
        IEnumerable<Appointment> appointments,
        DateTime date
    )
    {
        var workingDays = doctors.SelectMany(d => d.WorkingDays).ToList();
        return GetWorkingDayIsFullyBooked(workingDays, date, appointments);
    }

    private static bool GetWorkingDayIsFullyBooked(
        ICollection<WorkingDay> workingDays,
        DateTime date,
        IEnumerable<Appointment> appointments
    )
    {
        var workingRange = GetWorkingTimes(workingDays, date);
        var scheduledWorkTimeRange = appointments
            .Where(a => a.Start.Date == date.Date)
            .Select(a => TimeOnly.FromDateTime(a.Start)).ToList();
        return HasAll(workingRange.Times.ToList(), scheduledWorkTimeRange);
    }

    private static bool HasAll(IReadOnlyCollection<TimeOnly> workTimeRange,
        IEnumerable<TimeOnly> scheduledWorkTimeRange)
    {
        return workTimeRange.Intersect(scheduledWorkTimeRange).Count() == workTimeRange.Count;
    }
}
