using BlackSmith.Core.ExtensionMethods;
using BlackSmith.Core.Structs;
using BlackSmith.Domain.Interfaces;
using BlackSmith.Domain.Models;

namespace BlackSmith.Business.Components;

public class AppointmentsDoctorsBL
{
    private readonly IRepository<Appointment> _appointmentRepository;
    private readonly IRepository<Doctor> _doctorRepository;

    public AppointmentsDoctorsBL(IRepository<Appointment> appointmentRepository,
        IRepository<Doctor> doctorRepository)
    {
        _appointmentRepository = appointmentRepository;
        _doctorRepository = doctorRepository;
    }

    public async Task<IEnumerable<Doctor>> GetDoctorsBySpeciality(Speciality speciality)
    {
        return await _doctorRepository.GetAll(
            doctor => doctor.Speciality == speciality,
            doctor => doctor.Address,
            x => x.WorkingDays);
    }

    public bool GetSpecialityIsAvailable(IEnumerable<Doctor> doctors, DateTime day)
    {
        return doctors.SelectMany(doctor => doctor.WorkingDays)
            .Select(workingDay => workingDay.Day)
            .Distinct()
            .Contains(day.DayOfWeek);
    }

    public async Task<IEnumerable<Appointment>> GetUpcomingAppointmentsByDoctor(Doctor doctor)
    {
        return await _appointmentRepository.GetAll(x => x.DoctorId == doctor.Id && x.Start > DateTime.Now.Date);
    }

    public async Task<bool> GetDoctorIsFullyBookedOnSpecificDate(Doctor doctor, DateTime date)
    {
        var appointments = await GetAppointmentsByDoctorAndDay(doctor, date);
        return GetWorkingDayIsFullyBooked(doctor.WorkingDays.ToList(), date, appointments);
    }

    public TimeRange GetWorkingTimes(ICollection<WorkingDay> workingDays, DayOfWeek dayOfWeek)
    {
        var workStartTime = workingDays.Where(w => w.Day == dayOfWeek).Min(w => w.StartTime);
        var workFinishTime = workingDays.Where(w => w.Day == dayOfWeek).Max(w => w.EndTime);
        return new TimeRange(workStartTime, workFinishTime);
    }

    public async Task<IEnumerable<Appointment>> GetAppointmentsByDoctorAndDay(Doctor doctor, DateTime date)
    {
        return await _appointmentRepository.GetAll(a => a.Doctor == doctor && a.Start.DayOfWeek == date.DayOfWeek,
            a => a.Doctor,
            a => a.Patient);
    }

    public bool GetDoctorsAreFullyBookedOnSpecificDay(
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
        var workingRange = GetWorkingTimes(workingDays, date.DayOfWeek);
        var scheduledWorkTimeRange = appointments
            .Where(a => a.Start.Date == date.Date)
            .Select(a => TimeOnly.FromDateTime(a.Start)).ToList();
        return workingRange.Times.ToList().HasAll(scheduledWorkTimeRange);
    }
}
