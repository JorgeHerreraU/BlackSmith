using AutoMapper;
using BlackSmith.Domain.Models;
using BlackSmith.Service.DTOs;

namespace BlackSmith.Service.Profiles;

public class AppointmentsProfile : Profile
{
    public AppointmentsProfile()
    {
        CreateMap<Appointment, AppointmentDTO>().ReverseMap();
    }
}