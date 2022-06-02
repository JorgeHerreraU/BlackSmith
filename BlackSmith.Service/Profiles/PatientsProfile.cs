using AutoMapper;
using BlackSmith.Domain.Models;
using BlackSmith.Service.DTOs;

namespace BlackSmith.Service.Profiles;

public class PatientsProfile : Profile
{
    public PatientsProfile()
    {
        CreateMap<Patient, PatientDTO>().ReverseMap();
    }
}