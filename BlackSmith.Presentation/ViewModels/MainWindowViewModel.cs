using BlackSmith.Presentation.Events;
using BlackSmith.Presentation.Models;
using BlackSmith.Presentation.ViewModels.Doctors;
using BlackSmith.Presentation.ViewModels.Patients;
using BlackSmith.Presentation.ViewModels.Schedules;
using Prism.Events;
using Prism.Mvvm;

namespace BlackSmith.Presentation.ViewModels;

public class MainWindowViewModel : BindableBase
{
    private readonly DoctorCreateViewModel _doctorCreateViewModel;
    private readonly DoctorDetailViewModel _doctorDetailViewModel;
    private readonly DoctorEditViewModel _doctorEditViewModel;
    private readonly PatientCreateViewModel _patientCreateViewModel;
    private readonly PatientEditViewModel _patientEditViewModel;
    private readonly ScheduleCreateViewModel _scheduleCreateViewModel;
    private readonly ScheduleEditViewModel _scheduleEditViewModel;

    public MainWindowViewModel(
        IEventAggregator eventAggregator,
        PatientCreateViewModel patientCreateViewModel,
        PatientEditViewModel patientEditViewModel,
        DoctorEditViewModel doctorEditViewModel,
        DoctorDetailViewModel doctorDetailViewModel,
        DoctorCreateViewModel doctorCreateViewModel,
        ScheduleCreateViewModel scheduleCreateViewModel,
        ScheduleEditViewModel scheduleEditViewModel
    )
    {
        _patientCreateViewModel = patientCreateViewModel;
        _patientEditViewModel = patientEditViewModel;
        _doctorEditViewModel = doctorEditViewModel;
        _doctorDetailViewModel = doctorDetailViewModel;
        _doctorCreateViewModel = doctorCreateViewModel;
        _scheduleCreateViewModel = scheduleCreateViewModel;
        _scheduleEditViewModel = scheduleEditViewModel;

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
        _patientCreateViewModel.Initialize();
    }

    private void OnScheduleCreate()
    {
        _scheduleCreateViewModel.Initialize();
    }

    private void OnCreateDoctorEvent()
    {
        _doctorCreateViewModel.Initialize();
    }

    private void OnDoctorDetailsEvent(Doctor doctor)
    {
        _doctorDetailViewModel.Doctor = doctor;
    }

    private void OnPatientEditEvent(Patient patient)
    {
        _patientEditViewModel.Patient = patient;
    }

    private void OnDoctorEditEvent(Doctor doctor)
    {
        _doctorEditViewModel.Doctor = doctor;
    }

    private void OnScheduleEdit(Appointment appointment)
    {
        _scheduleEditViewModel.Appointment = appointment;
    }
}
