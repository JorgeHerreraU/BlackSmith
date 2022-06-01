using BlackSmith.Domain.Models;

namespace BlackSmith.Domain.Interfaces;

public interface IAppointmentService
{
    Task<IEnumerable<Appointment>> GetAppointments();
}