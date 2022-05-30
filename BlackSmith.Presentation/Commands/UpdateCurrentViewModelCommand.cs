using BlackSmith.Presentation.State.Navigators;
using System;
using System.Windows.Input;
using BlackSmith.Presentation.ViewModels.Factories;

namespace BlackSmith.Presentation.Commands;
public class UpdateCurrentViewModelCommand : ICommand
{
    private readonly INavigator _navigator;
    private readonly IViewModelAbstractFactory _viewModelAbstractFactory;

    public UpdateCurrentViewModelCommand(INavigator navigator, IViewModelAbstractFactory viewModelAbstractFactory)
    {
        _navigator = navigator;
        _viewModelAbstractFactory = viewModelAbstractFactory;
    }

    public event EventHandler? CanExecuteChanged;

    public bool CanExecute(object? parameter)
    {
        return true;
    }

    public void Execute(object? parameter)
    {
        if (parameter is not ViewType type) return;
        _navigator.CurrentViewModel = _viewModelAbstractFactory.CreateViewModel(type);
    }
}
