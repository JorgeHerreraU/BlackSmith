using AutoMapper;
using BlackSmith.Domain.Models;
using BlackSmith.Service.DTOs;

namespace BlackSmith.Service.Profiles;

public class AddressesProfile : Profile
{
    public AddressesProfile()
    {
        CreateMap<Address, AddressDTO>().ReverseMap();
    }
}
