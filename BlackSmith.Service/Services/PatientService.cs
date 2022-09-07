using AutoMapper;
using BlackSmith.Business.Components;
using BlackSmith.Domain.Models;
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

    public async Task<PatientDTO> CreatePatient(PatientDTO patient)
    {
        return _mapper.Map<PatientDTO>(await _patientsBl.CreatePatient(_mapper.Map<Patient>(patient)));
    }

    public async Task<PatientDTO> UpdatePatient(PatientDTO patient)
    {
        return _mapper.Map<PatientDTO>(await _patientsBl.UpdatePatient(_mapper.Map<Patient>(patient)));
    }

    public async Task<bool> DeletePatient(PatientDTO patient)
    {
        return await _patientsBl.DeletePatient(_mapper.Map<Patient>(patient));
    }
}
