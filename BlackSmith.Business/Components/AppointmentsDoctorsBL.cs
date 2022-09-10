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

    public TimeRange GetWorkingTimes(ICollection<WorkingDay> workingDays, DayOfWeek dayOfWeek)
    {
        var workStartTime = workingDays.Where(w => w.Day == dayOfWeek).Min(w => w.StartTime);
        var workFinishTime = workingDays.Where(w => w.Day == dayOfWeek).Max(w => w.EndTime);
        return new TimeRange(workStartTime, workFinishTime);
    }
}
