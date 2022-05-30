using BlackSmith.Presentation.Store;
using BlackSmith.Presentation.ViewModels;

namespace BlackSmith.Presentation.Commands;

public class UpdateSelectedViewModelCommand
{
    private readonly HomeViewModel _homeViewModel;
    private readonly NavigationStore _navigationStore;
    private readonly AppointmentViewModel _appointmentViewModel;

    public UpdateSelectedViewModelCommand(NavigationStore navigationStore, HomeViewModel homeViewModel,
        AppointmentViewModel appointmentViewModel)
    {
        _navigationStore = navigationStore;
        _homeViewModel = homeViewModel;
        _appointmentViewModel = appointmentViewModel;

        GoToHome = new RelayCommand(OpenHomeView);
        GoToAppointments = new RelayCommand(OpenAppointmentsView);
    }

    public RelayCommand GoToHome { get; set; }
    public RelayCommand GoToAppointments { get; set; }

    private void OpenAppointmentsView()
    {
        _navigationStore.SelectedViewModel = _appointmentViewModel;
    }

    private void OpenHomeView()
    {
        _navigationStore.SelectedViewModel = _homeViewModel;
    }
}