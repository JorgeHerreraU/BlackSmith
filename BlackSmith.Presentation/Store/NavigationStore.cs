using System;
using BlackSmith.Presentation.Modules.Appointments;

namespace BlackSmith.Presentation.Store;

public class NavigationStore
{
    private readonly AppointmentListViewModel _appointmentListViewModel;
    private BindableBase? _selectedAppointmentViewModel;
    private BindableBase? _selectedViewModel;

    public NavigationStore(AppointmentListViewModel appointmentListViewModel)
    {
        _appointmentListViewModel = appointmentListViewModel;
    }

    public BindableBase SelectedViewModel
    {
        get => _selectedViewModel!;
        set
        {
            _selectedViewModel?.Dispose();
            _selectedViewModel = value;
            SelectedViewModelChanged?.Invoke();
        }
    }

    public BindableBase SelectedAppointmentViewModel
    {
        get => _selectedAppointmentViewModel ?? _appointmentListViewModel;
        set
        {
            _selectedAppointmentViewModel?.Dispose();
            _selectedAppointmentViewModel = value;
            SelectedAppointmentViewModelChanged?.Invoke();
        }
    }

    public event Action? SelectedViewModelChanged;
    public event Action? SelectedAppointmentViewModelChanged;
}