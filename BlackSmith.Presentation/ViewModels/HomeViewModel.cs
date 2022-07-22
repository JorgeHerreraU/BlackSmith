using BlackSmith.Presentation.Commands;
using BlackSmith.Presentation.Interfaces;
using BlackSmith.Presentation.Services;
using BlackSmith.Presentation.Views.Pages;

namespace BlackSmith.Presentation.ViewModels;

public class HomeViewModel
{
    private readonly INavService _navService;

    public HomeViewModel(INavService navService)
    {
        _navService = navService;
        GoToDoctors = new RelayCommand(OnGoToDoctors);
    }

    public RelayCommand GoToDoctors { get; }

    private void OnGoToDoctors()
    {
        _navService.Navigate(new NavigationTriggeredEventArgs { Page = typeof(DoctorList) });
    }
}