using AutoMapper;
using BlackSmith.Business.Components;
using BlackSmith.Domain.Models;
using BlackSmith.Service.DTOs;
using BlackSmith.Service.Interfaces;

namespace BlackSmith.Service.Services;

public class DoctorService : IDoctorService
{
    private readonly DoctorsBL _doctorsBL;
    private readonly IMapper _mapper;

    public DoctorService(IMapper mapper, DoctorsBL doctorsBL)
    {
        _mapper = mapper;
        _doctorsBL = doctorsBL;
    }

    public async Task<IEnumerable<DoctorDTO>> GetDoctors()
    {
        return _mapper.Map<IEnumerable<DoctorDTO>>(await _doctorsBL.GetDoctors());
    }

    public async Task<PatientDTO> CreateDoctor(DoctorDTO doctor)
    {
        return _mapper.Map<PatientDTO>(await _doctorsBL.CreateDoctor(_mapper.Map<Doctor>(doctor)));
    }

    public async Task<DoctorDTO> UpdateDoctor(DoctorDTO doctor)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> DeleteDoctor(DoctorDTO doctor)
    {
        throw new NotImplementedException();
    }
}