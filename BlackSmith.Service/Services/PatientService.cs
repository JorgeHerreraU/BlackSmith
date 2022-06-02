using AutoMapper;
using BlackSmith.Business.Components;
using BlackSmith.Service.DTOs;
using BlackSmith.Service.Interfaces;

namespace BlackSmith.Service.Services;

public class PatientService : IPatientService
{
    private readonly IMapper _mapper;
    private readonly PatientsBL _patientsBl;

    public PatientService(PatientsBL patientsBl, IMapper mapper)
    {
        _patientsBl = patientsBl;
        _mapper = mapper;
    }

    public async Task<IEnumerable<PatientDTO>> GetPatients()
    {
        return _mapper.Map<IEnumerable<PatientDTO>>(await _patientsBl.GetPatients());
    }
}