using System;
using BlackSmith.Presentation.ViewModels;

namespace BlackSmith.Presentation.Store;

public class NavigationStore
{
    private readonly AppointmentIndexViewModel _indexViewModel;

    public NavigationStore(AppointmentIndexViewModel indexViewModel)
    {
        _indexViewModel = indexViewModel;
    }
    private BaseViewModel? _selectedAppointmentViewModel;
    private BaseViewModel? _selectedViewModel;

    public BaseViewModel SelectedViewModel
    {
        get => _selectedViewModel!;
        set
        {
            _selectedViewModel?.Dispose();
            _selectedViewModel = value;
            SelectedViewModelChanged?.Invoke();
        }
    }

    public BaseViewModel SelectedAppointmentViewModel
    {
        get => _selectedAppointmentViewModel ?? _indexViewModel;
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