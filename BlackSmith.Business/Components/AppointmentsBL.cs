using BlackSmith.Domain.Interfaces;
using BlackSmith.Domain.Models;

namespace BlackSmith.Business.Components;

public class AppointmentsBL
{
    private readonly IRepository<Appointment> _repository;

    public AppointmentsBL(IRepository<Appointment> repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<Appointment>> GetAppointments()
    {
        return await _repository.GetAll(x => x.Doctor, x => x.Patient);
    }
}