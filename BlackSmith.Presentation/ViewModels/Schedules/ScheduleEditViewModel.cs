using AutoMapper;
using BlackSmith.Presentation.Interfaces;
using BlackSmith.Presentation.Models;
using BlackSmith.Service.DTOs;
using BlackSmith.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Wpf.Ui.Mvvm.Contracts;

namespace BlackSmith.Presentation.ViewModels.Schedules;

public class ScheduleEditViewModel : ScheduleBaseViewModel
{
    private readonly IAppointmentService _appointmentService;
    private readonly IMapper _mapper;
    private Appointment _appointment = null!;

    public ScheduleEditViewModel(
        IModalService modalService,
        IPatientService patientService,
        IMapper mapper,
        INavigationService navigationService,
        IDoctorService doctorService,
        IAppointmentService appointmentService) : base(modalService,
        navigationService,
        appointmentService,
        mapper,
        doctorService,
        patientService)
    {
        _mapper = mapper;
        _appointmentService = appointmentService;
    }

    public override Appointment Appointment
    {
        get => _appointment;
        set
        {
            _appointment = value;
            Initialize();
            NotifyPropertyChanged();
        }
    }

    public override async void Initialize()
    {
        await LoadCollectionsData();
        ResetAvailableHours();
        SubscribeChanges();
        SetDefaultValues();
    }

    protected override void SetDefaultValues()
    {
        IsTouched = true;
        SelectedSpeciality = Doctors.FirstOrDefault(d => d.Id == Appointment.DoctorId)?.Speciality;
        SelectedPatient = Patients.FirstOrDefault(p => p.Id == Appointment.PatientId);
        SelectedDate = Appointment.Start;
        SelectedStartTime = TimeOnly.FromDateTime(Appointment.Start!.Value);
    }

    protected override async void OnSave()
    {
        try
        {
            await _appointmentService.UpdateAppointment(_mapper.Map<AppointmentDTO>(Appointment));
            OnGoBack();
        }
        catch (Exception ex)
        {
            OnSaveErrorHandler(ex);
        }
    }

    protected override void FilterDoctors()
    {
        base.FilterDoctors();
        SelectedDoctor = Doctors.FirstOrDefault(d => d.Id == Appointment.DoctorId);
    }

    protected override async void FilterTimes()
    {
        base.FilterTimes();
        if (Appointment.Start == null) return;
        var patient = _mapper.Map<PatientDTO>(Appointment.Patient);
        var appointmentDate = Appointment.Start.Value;
        var appointment = await _appointmentService.GetAppointmentByPatientAndDate(patient, appointmentDate);
        var hourIsMissing = CheckAppointmentHourIsMissing(DoctorAvailableHours, appointment, appointmentDate);
        if (hourIsMissing) AddAppointmentTimeToAvailableHours(DoctorAvailableHours, appointmentDate);
        SelectedStartTime = TimeOnly.FromDateTime(appointmentDate);
    }


    private bool CheckAppointmentHourIsMissing(ICollection<TimeOnly> availableHours,
        AppointmentDTO? appointment,
        DateTime appointmentDate)
    {
        var hourNotExists = !availableHours.Contains(TimeOnly.FromDateTime(appointmentDate));
        var isPatientAlreadyScheduled = appointment?.PatientId == Appointment.Patient?.Id;
        return hourNotExists && isPatientAlreadyScheduled;
    }

    private void AddAppointmentTimeToAvailableHours(List<TimeOnly> availableHours, DateTime selectedDate)
    {
        availableHours.Add(TimeOnly.FromDateTime(selectedDate));
        availableHours.Sort();
        AvailableHours = new ObservableCollection<TimeOnly>(availableHours);
    }
}
