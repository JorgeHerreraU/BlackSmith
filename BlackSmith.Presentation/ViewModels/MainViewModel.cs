using BlackSmith.Presentation.Store;

namespace BlackSmith.Presentation.ViewModels;

public class MainViewModel : BaseViewModel
{
    private readonly NavigationStore _navigationStore;

    public MainViewModel(NavigationStore navigationStore, NavbarViewModel navbarViewModel)
    {
        _navigationStore = navigationStore;
        NavbarViewModel = navbarViewModel;
        _navigationStore.SelectedViewModelChanged += OnSelectedViewModelChanged;
    }

    public NavbarViewModel NavbarViewModel { get; }

    public BaseViewModel SelectedViewModel => _navigationStore.SelectedViewModel;

    private void OnSelectedViewModelChanged()
    {
        OnPropertyChanged(nameof(SelectedViewModel));
    }
}