using BlackSmith.Core.Structs;
using BlackSmith.Service.DTOs;

namespace BlackSmith.Service.Interfaces;

public interface IAppointmentService
{
    Task<IEnumerable<AppointmentDTO>> GetAppointments();
    Task<List<TimeOnly>> GetAvailableHoursByDoctor(DoctorDTO doctorDTO, DateTime date);
    Task<AppointmentDTO?> GetAppointmentByPatientAndDate(PatientDTO patientDTO, DateTime date);
    Task<IEnumerable<DoctorDTO>> FilterAvailableDoctors(SpecialityDTO specialityDTO,
        DateRange dateRange,
        DateTime selectedDate,
        IEnumerable<DoctorDTO> doctors
    );
    Task<IEnumerable<DateTime>> GetSpecialityBlackoutDays(SpecialityDTO specialityDTO,
        DateRange dateRange,
        IEnumerable<DateTime> calendar);
    Task<AppointmentDTO> CreateAppointment(AppointmentDTO appointmentDTO);
    Task<AppointmentDTO> UpdateAppointment(AppointmentDTO appointmentDTO);
    Task<bool> DeleteAppointment(AppointmentDTO appointmentDTO);
}
