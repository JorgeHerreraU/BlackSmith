using AutoMapper;
using BlackSmith.Business.Components;
using BlackSmith.Core.Structs;
using BlackSmith.Domain.Models;
using BlackSmith.Service.DTOs;
using BlackSmith.Service.Interfaces;

namespace BlackSmith.Service.Services;

public class AppointmentService : IAppointmentService
{
    private readonly AppointmentsBL _appointmentsBl;
    private readonly IMapper _mapper;

    public AppointmentService(IMapper mapper, AppointmentsBL appointmentsBl)
    {
        _mapper = mapper;
        _appointmentsBl = appointmentsBl;
    }

    public async Task<IEnumerable<AppointmentDTO>> GetAppointments()
    {
        return _mapper.Map<IEnumerable<AppointmentDTO>>(await _appointmentsBl.GetAppointments());
    }

    public async Task<AppointmentDTO?> GetAppointmentByPatientAndDate(PatientDTO patientDTO, DateTime date)
    {
        return _mapper.Map<AppointmentDTO>(
            await _appointmentsBl.GetAppointmentByPatientAndDate(_mapper.Map<Patient>(patientDTO), date));
    }

    public async Task<IEnumerable<DateTime>> GetSpecialityBlackoutDays(SpecialityDTO specialityDTO,
        DateRange dateRange,
        IEnumerable<DateTime> calendar)
    {
        var speciality = (Speciality)Enum.Parse(typeof(Speciality), specialityDTO.ToString());
        var availableDays = await _appointmentsBl.GetAvailableDaysBySpeciality(speciality, dateRange).ToListAsync();
        return calendar.Except(availableDays);
    }

    public async Task<List<TimeOnly>> GetAvailableHoursByDoctor(DoctorDTO doctorDTO, DateTime date)
    {
        return await _appointmentsBl.GetAvailableHoursByDoctor(_mapper.Map<Doctor>(doctorDTO), date)
            .Select(TimeOnly.FromDateTime).ToListAsync();
    }

    public async Task<AppointmentDTO> CreateAppointment(AppointmentDTO appointmentDTO)
    {
        return _mapper.Map<AppointmentDTO>(
            await _appointmentsBl.CreateAppointment(_mapper.Map<Appointment>(appointmentDTO))
        );
    }

    public async Task<AppointmentDTO> UpdateAppointment(AppointmentDTO appointmentDTO)
    {
        return _mapper.Map<AppointmentDTO>(
            await _appointmentsBl.UpdateAppointment(_mapper.Map<Appointment>(appointmentDTO))
        );
    }

    public async Task<bool> DeleteAppointment(AppointmentDTO appointmentDTO)
    {
        return await _appointmentsBl.DeleteAppointment(_mapper.Map<Appointment>(appointmentDTO));
    }

    public async Task<IEnumerable<DoctorDTO>> FilterAvailableDoctors(SpecialityDTO specialityDTO,
        DateRange dateRange,
        DateTime selectedDate,
        IEnumerable<DoctorDTO> doctors
    )
    {
        var speciality = (Speciality)Enum.Parse(typeof(Speciality), specialityDTO.ToString());
        var availableDays = await _appointmentsBl
            .GetAvailableDaysBySpeciality(speciality, dateRange)
            .ToListAsync();
        return doctors
            .Where(IsSpeciality(speciality))
            .Where(IsDayOfWeek(selectedDate.DayOfWeek))
            .Where(IsDayAvailable(selectedDate, availableDays));
    }

    private static Func<DoctorDTO, bool> IsSpeciality(Speciality speciality)
    {
        return d => d.Speciality == speciality;
    }

    private static Func<DoctorDTO, bool> IsDayOfWeek(DayOfWeek dayOfWeek)
    {
        return d => d.WorkingDays.Select(w => w.Day).Contains(dayOfWeek);
    }

    private static Func<DoctorDTO, bool> IsDayAvailable(DateTime date, IEnumerable<DateTime> availableDays)
    {
        return _ => availableDays.Contains(date.Date);
    }
}
