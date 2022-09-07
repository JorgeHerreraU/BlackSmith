namespace BlackSmith.Service.DTOs;

public class AppointmentDTO
{
    public int Id { get; set; }
    public int PatientId { get; set; }
    public int DoctorId { get; set; }
    public PatientDTO Patient { get; set; } = new();
    public DoctorDTO Doctor { get; set; } = new();
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
}
