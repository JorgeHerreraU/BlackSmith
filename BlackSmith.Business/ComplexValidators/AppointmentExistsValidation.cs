using BlackSmith.Business.Interfaces;
using BlackSmith.Domain.Interfaces;
using BlackSmith.Domain.Models;

namespace BlackSmith.Business.ComplexValidators;

public class AppointmentExistsValidation : ICreateComplexValidation<Appointment>
{
    private readonly IRepository<Appointment> _repository;

    public AppointmentExistsValidation(IRepository<Appointment> repository)
    {
        _repository = repository;
    }
    public async Task ValidateAsync(Appointment appointment)
    {
        if (await AppointmentExists(appointment)) throw new ArgumentException("Appointment already exists");
    }

    private async Task<bool> AppointmentExists(Appointment appointment)
    {
        return await _repository.Get(a =>
            a.Start == appointment.Start
            && a.End == appointment.End
            && a.DoctorId == appointment.DoctorId) is not null;
    }
}
