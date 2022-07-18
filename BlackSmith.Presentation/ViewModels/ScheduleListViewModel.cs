using BlackSmith.Presentation.Commands;
using BlackSmith.Presentation.Interfaces;
using BlackSmith.Presentation.Services;
using BlackSmith.Presentation.Views.Pages.ScheduleCreate;
using Wpf.Ui.Mvvm;

namespace BlackSmith.Presentation.ViewModels;

public class ScheduleListViewModel : ViewModelBase
{
    private readonly INavService _navService;

    public ScheduleListViewModel(INavService navService)
    {
        _navService = navService;
        GoToCreate = new RelayCommand(OnCreate);
    }

    public RelayCommand GoToCreate { get; }

    private void OnCreate()
    {
        _navService.Navigate(new NavigationTriggeredEventArgs { Page = typeof(ScheduleCreateUserIdentification) });
    }
}