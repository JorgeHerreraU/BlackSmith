using BlackSmith.Presentation.Commands;
using BlackSmith.Presentation.Models;
using BlackSmith.Presentation.ViewModels;
using System.Windows.Input;
using BlackSmith.Presentation.ViewModels.Factories;

namespace BlackSmith.Presentation.State.Navigators;
public class Navigator : BindableBase, INavigator
{
    private BaseViewModel? _currentViewModel;
    public BaseViewModel CurrentViewModel
    {
        get => _currentViewModel!;
        set
        {
            _currentViewModel = value;
            OnPropertyChanged(nameof(CurrentViewModel));
        }
    }
    // ICommand is very important for data-binding
    public ICommand UpdateCurrentViewModelCommand { get; }

    public Navigator(IViewModelAbstractFactory viewModelAbstractFactory)
    {
        UpdateCurrentViewModelCommand = new UpdateCurrentViewModelCommand(this, viewModelAbstractFactory);
    }
    
}
