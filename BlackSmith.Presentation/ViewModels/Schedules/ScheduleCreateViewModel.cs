using AutoMapper;
using BlackSmith.Presentation.Enums;
using BlackSmith.Presentation.Interfaces;
using BlackSmith.Presentation.Models;
using BlackSmith.Service.DTOs;
using BlackSmith.Service.Interfaces;
using System;
using System.Linq;
using Wpf.Ui.Mvvm.Contracts;

namespace BlackSmith.Presentation.ViewModels.Schedules;

public class ScheduleCreateViewModel : ScheduleBaseViewModel
{
    private readonly IAppointmentService _appointmentService;
    private readonly IMapper _mapper;

    public ScheduleCreateViewModel(
        IAppointmentService appointmentService,
        IPatientService patientService,
        IModalService modalService,
        IMapper mapper,
        INavigationService navigationService,
        IDoctorService doctorService) : base(modalService,
        navigationService,
        appointmentService,
        mapper,
        doctorService,
        patientService)
    {
        _appointmentService = appointmentService;
        _mapper = mapper;
    }

    public override async void Initialize()
    {
        Appointment = new Appointment();
        await LoadCollectionsData();
        ResetAvailableHours();
        SubscribeChanges();
        SetDefaultValues();
    }

    protected override void SetDefaultValues()
    {
        IsTouched = false;
        SelectedSpeciality = Speciality.GeneralPractice;
        SelectedPatient = null!;
        SelectedDoctor = null!;
        SelectedDate = null!;
        SelectedStartTime = AvailableHours.First();
    }

    protected override void FilterTimes()
    {
        base.FilterTimes();
        SelectedStartTime = AvailableHours.First();
    }

    protected override async void OnSave()
    {
        try
        {
            await _appointmentService.CreateAppointment(_mapper.Map<AppointmentDTO>(Appointment));
            OnGoBack();
        }
        catch (Exception ex)
        {
            OnSaveErrorHandler(ex);
        }
    }
}
