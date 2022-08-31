using System;
using System.ComponentModel.DataAnnotations;

namespace BlackSmith.Presentation.Models;

public class Appointment : ValidatableBase
{
    public double Duration { get; init; } = 60;
    private DateTime? _start = null!;
    private Patient _patient = null!;
    private Doctor _doctor = null!;

    public int Id { get; set; }
    public int PatientId { get; set; }
    public int DoctorId { get; set; }
    [Required]
    public Patient Patient
    {
        get => _patient;
        set
        {
            _patient = value;
            NotifyPropertyChanged();
        }
    }
    [Required]
    public Doctor Doctor
    {
        get => _doctor;
        set
        {
            _doctor = value;
            NotifyPropertyChanged();
        }
    }
    [Required]
    public DateTime? Start
    {
        get => _start;
        set
        {
            _start = value;
            NotifyPropertyChanged();
        }
    }
    public DateTime End { get; set; }
}