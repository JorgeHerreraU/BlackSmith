using BlackSmith.Business.Helpers;
using BlackSmith.Domain.Interfaces;
using BlackSmith.Domain.Models;

namespace BlackSmith.Business.Components;

public class AppointmentsBL
{
    private readonly IRepository<Appointment> _appointmentsRepository;
    private readonly DoctorsBL _doctorsBL;

    public AppointmentsBL(IRepository<Appointment> repository, DoctorsBL doctorsBL)
    {
        _appointmentsRepository = repository;
        _doctorsBL = doctorsBL;
    }

    public async Task<IEnumerable<Appointment>> GetAppointments()
    {
        return await _appointmentsRepository.GetAll(
            appointment => appointment.Doctor,
            appointment => appointment.Patient);
    }

    public async IAsyncEnumerable<DateTime> GetAvailableDaysByDoctorsSpeciality(Speciality speciality,
        DateTime start, DateTime end)
    {
        var days = DateHelper.GetDateRange(start, end);
        var doctors = await _doctorsBL.GetDoctorsBySpeciality(speciality);
        var appointments = await GetAppointmentsByDoctorsSpeciality(speciality);

        foreach (var day in days)
        {
            var isAWorkingDay = _doctorsBL.GetSpecialityIsAvailable(doctors, day);

            if (isAWorkingDay)
            {
                var isDayFullyBooked = GetDoctorsAreFullyBookedOnSpecificDay(doctors, appointments, day);

                if (!isDayFullyBooked)
                    yield return day;
            }
        }
    }

    public async Task<bool> GetDoctorIsFullyBookedOnSpecificDay(Doctor doctor, DateTime date)
    {
        var appointments = await GetAppointmentsByDoctorAndDay(doctor, date);
        return GetWorkingDayIsFullyBooked(doctor.WorkingDays, date, appointments);
    }

    public async Task<IEnumerable<Appointment>> GetAppointmentsByDoctorsSpeciality(Speciality speciality)
    {
        return await _appointmentsRepository.GetAll(appointment => appointment.Doctor.Speciality == speciality,
            appointment => appointment.Doctor, appointment => appointment.Patient);
    }

    public async Task<Appointment> UpdateAppointment(Appointment appointment)
    {
        return await _appointmentsRepository.Update(appointment);
    }

    public async Task<Appointment> Add(Appointment appointment)
    {
        return await _appointmentsRepository.Add(appointment);
    }


    private async Task<IEnumerable<Appointment>> GetAppointmentsByDoctorAndDay(Doctor doctor, DateTime date)
    {
        return await _appointmentsRepository.GetAll(
            appointment => appointment.Doctor == doctor && appointment.Start.DayOfWeek == date.DayOfWeek,
            appointment => appointment.Doctor,
            appointment => appointment.Patient);
    }

    private static bool GetDoctorsAreFullyBookedOnSpecificDay(IEnumerable<Doctor> doctors,
        IEnumerable<Appointment> appointments,
        DateTime date)
    {
        var workingDays = doctors.SelectMany(doctor => doctor.WorkingDays);
        return GetWorkingDayIsFullyBooked(workingDays, date, appointments);
    }

    private static bool GetWorkingDayIsFullyBooked(IEnumerable<WorkingDay> workingDays,
        DateTime date, IEnumerable<Appointment> appointments)
    {
        var workStartTime = workingDays.Where(workingDay => workingDay.Day == date.DayOfWeek)
            .Min(workingDay => workingDay.StartTime);
        var workFinishTime = workingDays.Where(workingDay => workingDay.Day == date.DayOfWeek)
            .Max(workingDay => workingDay.EndTime);
        var workTimeRange = TimeHelper.GetTimeRange(workStartTime, workFinishTime);
        var scheduledWorkTimeRange = appointments.Where(appointment => appointment.Start.Date == date.Date)
            .Select(x => TimeOnly.FromDateTime(x.Start));
        return HasAll(workTimeRange, scheduledWorkTimeRange);
    }

    private static bool HasAll(IEnumerable<TimeOnly> workTimeRange, IEnumerable<TimeOnly> scheduledWorkTimeRange)
    {
        return workTimeRange.Intersect(scheduledWorkTimeRange)
            .Count() == workTimeRange.Count();
    }
}