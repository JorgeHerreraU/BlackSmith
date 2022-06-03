using BlackSmith.Presentation.Commands;
using BlackSmith.Presentation.Store;

namespace BlackSmith.Presentation.Modules.Patients;

public class PatientViewModel : BindableBase
{
    private readonly NavigationStore _navigationStore;
    private readonly PatientCreateViewModel _patientCreateViewModel;
    private readonly PatientListViewModel _patientListViewModel;

    public PatientViewModel(
        PatientCreateViewModel patientCreateViewModel,
        PatientListViewModel patientListViewModel,
        NavigationStore navigationStore
    )
    {
        _patientCreateViewModel = patientCreateViewModel;
        _patientListViewModel = patientListViewModel;
        _navigationStore = navigationStore;
        _navigationStore.SelectedAppointmentViewModelChanged += OnSelectedPatientViewModelChanged;
        GoToCreate = new RelayCommand(OpenCreatePatient);
        GoToList = new RelayCommand(OpenListPatient);
    }

    public RelayCommand GoToCreate { get; }
    public RelayCommand GoToList { get; }
    public BindableBase SelectedPatientViewModel => _navigationStore.SelectedPatientViewModel;

    private void OpenListPatient()
    {
        _navigationStore.SelectedPatientViewModel = _patientListViewModel;
    }

    private void OpenCreatePatient()
    {
        _navigationStore.SelectedPatientViewModel = _patientCreateViewModel;
    }

    private void OnSelectedPatientViewModelChanged()
    {
        OnPropertyChanged(nameof(SelectedPatientViewModel));
    }
}