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

    public MainWindowViewModel(IEventAggregator eventAggregator,
        DoctorEditViewModel doctorEditViewModel,
        PatientEditViewModel patientEditViewModel,
        DoctorDetailViewModel doctorDetailViewModel,
        DoctorCreateViewModel doctorCreateViewModel)
    {
        _doctorEditViewModel = doctorEditViewModel;
        _patientEditViewModel = patientEditViewModel;
        _doctorDetailViewModel = doctorDetailViewModel;
        _doctorCreateViewModel = doctorCreateViewModel;

        eventAggregator.GetEvent<CreateDoctorEvent>().Subscribe(OnCreateDoctorEvent);
        eventAggregator.GetEvent<EditDoctorEvent>().Subscribe(OnDoctorEditEvent);
        eventAggregator.GetEvent<EditPatientEvent>().Subscribe(OnPatientEditEvent);
        eventAggregator.GetEvent<DetailsDoctorEvent>().Subscribe(OnDoctorDetailsEvent);
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