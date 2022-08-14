using BlackSmith.Presentation.Events;
using BlackSmith.Presentation.Models;
using Prism.Events;
using Prism.Mvvm;

namespace BlackSmith.Presentation.ViewModels;

public class MainWindowViewModel : BindableBase
{
    private readonly DoctorCreateViewModel _doctorCreateViewModel;
    private readonly DoctorDetailViewModel _doctorDetailViewModel;
    private readonly DoctorEditViewModel _doctorEditViewModel;
    private readonly PatientEditViewModel _patientEditViewModel;
    private readonly ScheduleCreateViewModel _scheduleCreateViewModel;

    public MainWindowViewModel(IEventAggregator eventAggregator,
        DoctorEditViewModel doctorEditViewModel,
        PatientEditViewModel patientEditViewModel,
        DoctorDetailViewModel doctorDetailViewModel,
        DoctorCreateViewModel doctorCreateViewModel,
        ScheduleCreateViewModel scheduleCreateViewModel)
    {
        _doctorEditViewModel = doctorEditViewModel;
        _patientEditViewModel = patientEditViewModel;
        _doctorDetailViewModel = doctorDetailViewModel;
        _doctorCreateViewModel = doctorCreateViewModel;
        _scheduleCreateViewModel = scheduleCreateViewModel;

        eventAggregator.GetEvent<CreateDoctorEvent>().Subscribe(OnCreateDoctorEvent);
        eventAggregator.GetEvent<EditDoctorEvent>().Subscribe(OnDoctorEditEvent);

        eventAggregator.GetEvent<EditPatientEvent>().Subscribe(OnPatientEditEvent);
        eventAggregator.GetEvent<DetailsDoctorEvent>().Subscribe(OnDoctorDetailsEvent);

        eventAggregator.GetEvent<CreateScheduleEvent>().Subscribe(OnCreateScheduleEvent);
    }

    private void OnCreateScheduleEvent()
    {
        _scheduleCreateViewModel.SetNewSchedule();
    }

    private void OnCreateDoctorEvent()
    {
        _doctorCreateViewModel.SetNewDoctor();
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
}