using BlackSmith.Presentation.Controls;
using BlackSmith.Presentation.Store;

namespace BlackSmith.Presentation;

public class MainWindowViewModel : BindableBase
{
    private readonly NavigationStore _navigationStore;

    public MainWindowViewModel(NavigationStore navigationStore, NavbarViewModel navbarViewModel)
    {
        _navigationStore = navigationStore;
        NavbarViewModel = navbarViewModel;
        _navigationStore.SelectedViewModelChanged += OnSelectedViewModelChanged;
    }

    public NavbarViewModel NavbarViewModel { get; }

    public BindableBase SelectedViewModel => _navigationStore.SelectedViewModel;

    private void OnSelectedViewModelChanged()
    {
        OnPropertyChanged(nameof(SelectedViewModel));
    }
}