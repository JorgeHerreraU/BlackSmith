using BlackSmith.Presentation.Commands;
using BlackSmith.Presentation.Store;

namespace BlackSmith.Presentation.ViewModels;

public class AppointmentViewModel : BaseViewModel
{
    private readonly AppointmentCreateViewModel _appointmentCreateViewModel;
    private readonly AppointmentIndexViewModel _appointmentIndexViewModel;
    private readonly NavigationStore _navigationStore;

    public AppointmentViewModel(NavigationStore navigationStore, AppointmentCreateViewModel appointmentCreateViewModel,
        AppointmentIndexViewModel appointmentIndexViewModel)
    {
        _navigationStore = navigationStore;
        _appointmentCreateViewModel = appointmentCreateViewModel;
        _appointmentIndexViewModel = appointmentIndexViewModel;
        _navigationStore.SelectedAppointmentViewModelChanged += OnSelectedAppointmentViewModelChanged;
        GoToCreate = new RelayCommand(OpenCreateAppointment);
        GoToIndex = new RelayCommand(OpenIndexAppointment);
    }

    public BaseViewModel SelectedAppointmentViewModel => _navigationStore.SelectedAppointmentViewModel;
    public RelayCommand GoToCreate { get; }
    public RelayCommand GoToIndex { get; }

    private void OpenIndexAppointment()
    {
        _navigationStore.SelectedAppointmentViewModel = _appointmentIndexViewModel;
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