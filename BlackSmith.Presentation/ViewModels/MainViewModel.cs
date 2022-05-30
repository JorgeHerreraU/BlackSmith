using BlackSmith.Presentation.Store;

namespace BlackSmith.Presentation.ViewModels;

public class MainViewModel : BaseViewModel
{
    private readonly NavigationStore _navigationStore;

    public MainViewModel(NavigationStore navigationStore)
    {
        _navigationStore = navigationStore;
        _navigationStore.SelectedViewModelChanged += OnSelectedViewModelChanged;
    }

    public BaseViewModel SelectedViewModel => _navigationStore.SelectedViewModel;

    private void OnSelectedViewModelChanged()
    {
        OnPropertyChanged(nameof(SelectedViewModel));
    }
}