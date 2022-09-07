using AutoMapper;
using BlackSmith.Domain.Models;
using BlackSmith.Service.DTOs;

namespace BlackSmith.Service.Profiles;

public class DoctorsProfile : Profile
{
    public DoctorsProfile()
    {
        CreateMap<Doctor, DoctorDTO>().ReverseMap();
        CreateMap<WorkingDay, WorkingDayDTO>().ReverseMap();
    }
}
