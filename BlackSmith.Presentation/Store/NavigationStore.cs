using System;
using BlackSmith.Presentation.ViewModels;

namespace BlackSmith.Presentation.Store;

public class NavigationStore
{
    private BaseViewModel? _selectedViewModel;

    public BaseViewModel SelectedViewModel
    {
        get => _selectedViewModel = new HomeViewModel();
        set
        {
            _selectedViewModel?.Dispose();
            _selectedViewModel = value;
            SelectedViewModelChanged?.Invoke();
        }
    }

    public event Action? SelectedViewModelChanged;
}