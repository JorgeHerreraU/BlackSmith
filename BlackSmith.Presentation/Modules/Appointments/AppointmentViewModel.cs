using BlackSmith.Presentation.Commands;
using BlackSmith.Presentation.Store;
using BlackSmith.Service.DTOs;

namespace BlackSmith.Presentation.Modules.Appointments;

public class AppointmentViewModel : BindableBase
{
    private readonly AppointmentCreateEditViewModel _appointmentCreateEditViewModel;
    private readonly AppointmentListViewModel _appointmentListViewModel;
    private readonly NavigationStore _navigationStore;

    public AppointmentViewModel(
        NavigationStore navigationStore,
        AppointmentCreateEditViewModel appointmentCreateEditViewModel,
        AppointmentListViewModel appointmentListViewModel)
    {
        _navigationStore = navigationStore;
        _appointmentCreateEditViewModel = appointmentCreateEditViewModel;
        _appointmentListViewModel = appointmentListViewModel;
        _appointmentListViewModel.EditAppointmentRequested += GoToEdit;
        _navigationStore.SelectedAppointmentViewModelChanged += OnSelectedAppointmentViewModelChanged;
        GoToCreate = new RelayCommand(OpenCreateAppointment);
        GoToList = new RelayCommand(OpenListAppointment);
    }

    public BindableBase SelectedAppointmentViewModel => _navigationStore.SelectedAppointmentViewModel;
    public RelayCommand GoToCreate { get; }
    public RelayCommand GoToList { get; }

    private void GoToEdit(AppointmentDTO appointment)
    {
        _appointmentCreateEditViewModel.EditMode = true;
        _appointmentCreateEditViewModel.SetAppointment(appointment);
        _navigationStore.SelectedAppointmentViewModel = _appointmentCreateEditViewModel;
    }

    private void OpenListAppointment()
    {
        _navigationStore.SelectedAppointmentViewModel = _appointmentListViewModel;
    }

    private void OpenCreateAppointment()
    {
        _navigationStore.SelectedAppointmentViewModel = _appointmentCreateEditViewModel;
    }

    private void OnSelectedAppointmentViewModelChanged()
    {
        OnPropertyChanged(nameof(SelectedAppointmentViewModel));
    }
}