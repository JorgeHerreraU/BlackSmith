using AutoMapper;
using BlackSmith.Business.Components;
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

    public async Task<AppointmentDTO> UpdateAppointment(AppointmentDTO appointmentDTO)
    {
        return _mapper.Map<AppointmentDTO>(
            await _appointmentsBl.UpdateAppointment(_mapper.Map<Appointment>(appointmentDTO)));
    }

    public async Task<AppointmentDTO> AddAppointment(AppointmentDTO appointmentDTO)
    {
        return _mapper.Map<AppointmentDTO>(
            await _appointmentsBl.Add(_mapper.Map<Appointment>(appointmentDTO)));
    }
}