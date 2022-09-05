using BlackSmith.Service.DTOs;

namespace BlackSmith.Service.Interfaces;

public interface IAppointmentService
{
    Task<IEnumerable<AppointmentDTO>> GetAppointments();
    Task<AppointmentDTO> UpdateAppointment(AppointmentDTO appointmentDTO);
    Task<AppointmentDTO> CreateAppointment(AppointmentDTO appointmentDTO);
    Task<IEnumerable<DateTime>> GetAvailableDaysByDoctorsSpeciality(
        SpecialityDTO specialityDTO,
        DateTime start,
        DateTime end
    );
    Task<bool> GetDoctorFullyBooked(DoctorDTO doctorDTO, DateTime date);
    Task<bool> DeleteAppointment(AppointmentDTO appointmentDTO);
}
