using System;
using BlackSmith.Service.DTOs;

namespace BlackSmith.Presentation.Modules.Appointments;

public class SimpleEditableAppointment : ValidatableBase
{
    private string _description = "";
    private DoctorDTO _doctor = new();
    private int _doctorId;
    private DateTime _end;
    private int _id;

    private PatientDTO _patient = new();
    private int _patientId;
    private DateTime _start;

    private string _title = "";

    public int Id
    {
        get => _id;
        set => SetPropertyChanged(ref _id, value);
    }

    public int PatientId
    {
        get => _patientId;
        set => SetPropertyChanged(ref _patientId, value);
    }

    public int DoctorId
    {
        get => _doctorId;
        set => SetPropertyChanged(ref _doctorId, value);
    }

    public PatientDTO Patient
    {
        get => _patient;
        set => SetPropertyChanged(ref _patient, value);
    }

    public DoctorDTO Doctor
    {
        get => _doctor;
        set => SetPropertyChanged(ref _doctor, value);
    }

    public string Title
    {
        get => _title;
        set => SetPropertyChanged(ref _title, value);
    }

    public string Description
    {
        get => _description;
        set => SetPropertyChanged(ref _description, value);
    }

    public DateTime Start
    {
        get => _start;
        set => SetPropertyChanged(ref _start, value);
    }

    public DateTime End
    {
        get => _end;
        set => SetPropertyChanged(ref _end, value);
    }
}