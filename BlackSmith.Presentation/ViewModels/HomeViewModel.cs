using BlackSmith.Presentation.Commands;
using BlackSmith.Presentation.Views.Pages;
using Prism.Mvvm;
using Wpf.Ui.Mvvm.Contracts;

namespace BlackSmith.Presentation.ViewModels;

public class HomeViewModel : BindableBase
{
    private readonly INavigationService _navigationService;

    public HomeViewModel(INavigationService navigationService)
    {
        _navigationService = navigationService;

        GoToDoctors = new RelayCommand(OnGoToDoctors);
        GoToPatients = new RelayCommand(OnGoToPatients);
        GoToSchedules = new RelayCommand(OnGoToSchedules);
    }

    public RelayCommand GoToDoctors { get; }
    public RelayCommand GoToPatients { get; }
    public RelayCommand GoToSchedules { get; }

    private void OnGoToSchedules()
    {
        _navigationService.Navigate(typeof(ScheduleList));
    }

    private void OnGoToPatients()
    {
        _navigationService.Navigate(typeof(PatientList));
    }

    private void OnGoToDoctors()
    {
        _navigationService.Navigate(typeof(DoctorList));
    }
}