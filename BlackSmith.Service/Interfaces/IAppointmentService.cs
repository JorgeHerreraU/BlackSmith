using BlackSmith.Service.DTOs;

namespace BlackSmith.Service.Interfaces;

public interface IAppointmentService
{
    Task<IEnumerable<AppointmentDTO>> GetAppointments();
    Task<AppointmentDTO> UpdateAppointment(AppointmentDTO appointmentDTO);
    Task<AppointmentDTO> AddAppointment(AppointmentDTO appointmentDTO);
}