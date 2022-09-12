using BlackSmith.Presentation.Events;
using BlackSmith.Presentation.Models;
using BlackSmith.Presentation.ViewModels.Doctors;
using BlackSmith.Presentation.ViewModels.Patients;
using BlackSmith.Presentation.ViewModels.Schedules;
using Microsoft.Extensions.DependencyInjection;
using Prism.Events;
using Prism.Mvvm;
using System;

namespace BlackSmith.Presentation.ViewModels;

public class MainWindowViewModel : BindableBase
{
    private readonly IServiceProvider _serviceProvider;

    public MainWindowViewModel(IEventAggregator eventAggregator, IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;

        eventAggregator.GetEvent<CreateDoctorEvent>().Subscribe(OnCreateDoctorEvent);
        eventAggregator.GetEvent<EditDoctorEvent>().Subscribe(OnDoctorEditEvent);
        eventAggregator.GetEvent<DetailsDoctorEvent>().Subscribe(OnDoctorDetailsEvent);

        eventAggregator.GetEvent<CreatePatientEvent>().Subscribe(OnPatientCreate);
        eventAggregator.GetEvent<EditPatientEvent>().Subscribe(OnPatientEditEvent);

        eventAggregator.GetEvent<CreateScheduleEvent>().Subscribe(OnScheduleCreate);
        eventAggregator.GetEvent<EditScheduleEvent>().Subscribe(OnScheduleEdit);
    }

    private void OnPatientCreate()
    {
        _serviceProvider.GetRequiredService<PatientCreateViewModel>().Initialize();
    }

    private void OnScheduleCreate()
    {
        _serviceProvider.GetRequiredService<ScheduleCreateViewModel>().Initialize();
    }

    private void OnCreateDoctorEvent()
    {
        _serviceProvider.GetRequiredService<DoctorCreateViewModel>().Initialize();
    }

    private void OnDoctorDetailsEvent(Doctor doctor)
    {
        _serviceProvider.GetRequiredService<DoctorDetailViewModel>().Doctor = doctor;
    }

    private void OnPatientEditEvent(Patient patient)
    {
        _serviceProvider.GetRequiredService<PatientEditViewModel>().Patient = patient;
    }

    private void OnDoctorEditEvent(Doctor doctor)
    {
        _serviceProvider.GetRequiredService<DoctorEditViewModel>().Doctor = doctor;
    }

    private void OnScheduleEdit(Appointment appointment)
    {
        _serviceProvider.GetRequiredService<ScheduleEditViewModel>().Appointment = appointment;
    }
}
