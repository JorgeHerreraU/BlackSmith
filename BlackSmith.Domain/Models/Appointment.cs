namespace BlackSmith.Domain.Models;

public class Appointment : BaseEntity
{
    public int PatientId { get; set; }
    public int DoctorId { get; set; }
    public Patient Patient { get; set; } = new();
    public Doctor Doctor { get; set; } = new();
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
    public bool IsConfirmed { get; set; }
}
