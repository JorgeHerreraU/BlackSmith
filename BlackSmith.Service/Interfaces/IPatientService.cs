using BlackSmith.Service.DTOs;

namespace BlackSmith.Service.Interfaces;

public interface IPatientService
{
    Task<IEnumerable<PatientDTO>> GetPatients();
    Task<PatientDTO> CreatePatient(PatientDTO patient);
    Task<PatientDTO> UpdatePatient(PatientDTO patient);
    Task<bool> DeletePatient(PatientDTO patient);
}
