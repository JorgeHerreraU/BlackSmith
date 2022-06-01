using System;

namespace BlackSmith.Presentation.Models;

public class Appointment : BindableBase
{
    public int Id { get; set; }
    public int PatientId { get; set; }
    public int DoctorId { get; set; }
    public Patient Patient { get; set; } = new();
    public Doctor Doctor { get; set; } = new();
    public string Title { get; set; } = "";
    public string Description { get; set; } = "";
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
}