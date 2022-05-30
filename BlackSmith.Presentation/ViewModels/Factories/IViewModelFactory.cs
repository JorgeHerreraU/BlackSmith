namespace BlackSmith.Presentation.ViewModels.Factories;

public interface IViewModelFactory<out T> where T : BaseViewModel
{
    T CreateViewModel();
}