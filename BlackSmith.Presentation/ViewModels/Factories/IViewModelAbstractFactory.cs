using BlackSmith.Presentation.State.Navigators;

namespace BlackSmith.Presentation.ViewModels.Factories;
public interface IViewModelAbstractFactory
{
    BaseViewModel CreateViewModel(ViewType viewType);
}
