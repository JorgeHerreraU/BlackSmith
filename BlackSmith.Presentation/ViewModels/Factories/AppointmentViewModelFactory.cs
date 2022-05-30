namespace BlackSmith.Presentation.ViewModels.Factories;

public class AppointmentViewModelFactory : IViewModelFactory<AppointmentViewModel>
{
    public AppointmentViewModel CreateViewModel()
    {
        return new AppointmentViewModel();
    }
}