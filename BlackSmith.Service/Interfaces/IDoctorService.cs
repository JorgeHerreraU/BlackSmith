using BlackSmith.Service.DTOs;

namespace BlackSmith.Service.Interfaces;

public interface IDoctorService
{
    Task<IEnumerable<DoctorDTO>> GetDoctors();
    Task<PatientDTO> CreateDoctor(DoctorDTO doctor);
    Task<DoctorDTO> UpdateDoctor(DoctorDTO doctor);
    Task<bool> DeleteDoctor(DoctorDTO doctor);
}