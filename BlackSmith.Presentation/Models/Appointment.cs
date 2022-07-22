using System;

namespace BlackSmith.Presentation.Models;

public class Appointment
{
    public int Id { get; set; }
    public int PatientId { get; set; }
    public int DoctorId { get; set; }
    public Patient Patient { get; set; } = new();
    public Doctor Doctor { get; set; } = new();
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
}