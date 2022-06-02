using BlackSmith.Service.DTOs;

namespace BlackSmith.Service.Interfaces;

public interface IPatientService
{
    Task<IEnumerable<PatientDTO>> GetPatients();
}