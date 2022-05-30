namespace BlackSmith.Presentation.ViewModels.Factories;

public class HomeViewModelFactory : IViewModelFactory<HomeViewModel>
{
    public HomeViewModel CreateViewModel()
    {
        return new HomeViewModel();
    }
}