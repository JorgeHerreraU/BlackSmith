using BlackSmith.Presentation.Views.Pages.Doctors;
using BlackSmith.Presentation.Views.Pages.Patients;
using BlackSmith.Presentation.Views.Pages.Schedules;
using Prism.Commands;
using Prism.Mvvm;
using Wpf.Ui.Mvvm.Contracts;

namespace BlackSmith.Presentation.ViewModels.Home;

public class HomeViewModel : BindableBase
{
    private readonly INavigationService _navigationService;

    public HomeViewModel(INavigationService navigationService)
    {
        _navigationService = navigationService;

        GoToDoctors = new DelegateCommand(OnGoToDoctors);
        GoToPatients = new DelegateCommand(OnGoToPatients);
        GoToSchedules = new DelegateCommand(OnGoToSchedules);
    }

    public DelegateCommand GoToDoctors { get; }
    public DelegateCommand GoToPatients { get; }
    public DelegateCommand GoToSchedules { get; }

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
