using BlackSmith.Presentation.Commands;
using BlackSmith.Presentation.Store;

namespace BlackSmith.Presentation.Modules.Appointments;

public class AppointmentViewModel : BindableBase
{
    private readonly AppointmentCreateViewModel _appointmentCreateViewModel;
    private readonly AppointmentListViewModel _appointmentListViewModel;
    private readonly NavigationStore _navigationStore;

    public AppointmentViewModel(
        NavigationStore navigationStore,
        AppointmentCreateViewModel appointmentCreateViewModel,
        AppointmentListViewModel appointmentListViewModel)
    {
        _navigationStore = navigationStore;
        _appointmentCreateViewModel = appointmentCreateViewModel;
        _appointmentListViewModel = appointmentListViewModel;
        _navigationStore.SelectedAppointmentViewModelChanged += OnSelectedAppointmentViewModelChanged;
        GoToCreate = new RelayCommand(OpenCreateAppointment);
        GoToList = new RelayCommand(OpenListAppointment);
    }

    public BindableBase SelectedAppointmentViewModel => _navigationStore.SelectedAppointmentViewModel;
    public RelayCommand GoToCreate { get; }
    public RelayCommand GoToList { get; }

    private void OpenListAppointment()
    {
        _navigationStore.SelectedAppointmentViewModel = _appointmentListViewModel;
    }

    private void OpenCreateAppointment()
    {
        _navigationStore.SelectedAppointmentViewModel = _appointmentCreateViewModel;
    }

    private void OnSelectedAppointmentViewModelChanged()
    {
        OnPropertyChanged(nameof(SelectedAppointmentViewModel));
    }
}