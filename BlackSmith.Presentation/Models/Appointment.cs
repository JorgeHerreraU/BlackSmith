using BlackSmith.Core.ExtensionMethods;
using System;
using System.ComponentModel.DataAnnotations;

namespace BlackSmith.Presentation.Models;

public class Appointment : ValidatableBase
{
    public const double Duration = 60;
    public const int StartingHour = 9;
    public const int EndingHour = 18;
    private Doctor? _doctor;
    private int _doctorId;
    private DateTime _end;
    private int _id;
    private Patient? _patient;
    private int _patientId;
    private DateTime? _start;
    private bool _isConfirmed;
    public int Id
    {
        get => _id;
        set
        {
            _id = value;
            NotifyPropertyChanged();
        }
    }

    [Required]
    public int PatientId
    {
        get => _patientId;
        set
        {
            _patientId = value;
            NotifyPropertyChanged();
        }
    }

    [Required]
    public int DoctorId
    {
        get => _doctorId;
        set
        {
            _doctorId = value;
            NotifyPropertyChanged();
        }
    }

    [Required]
    public Patient? Patient
    {
        get => _patient;
        set
        {
            _patient = value;
            NotifyPropertyChanged();
        }
    }

    [Required]
    public Doctor? Doctor
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

    public DateTime End
    {
        get => _end;
        set
        {
            _end = value;
            NotifyPropertyChanged();
        }
    }

    public bool IsConfirmed
    {
        get => _isConfirmed;
        set
        {
            _isConfirmed = value;
            NotifyPropertyChanged();
        }
    }

    public bool IsCompleted
    {
        get => End < DateTime.Now;
    }

    public override string ToString()
    {
        return $"Schedule: {Start:yyyy/MM/dd HH:mm}, \nPatient: {Patient?.FullName}, \nSpeciality: {Doctor?.Speciality.GetEnumDescription()}";
    }
}
