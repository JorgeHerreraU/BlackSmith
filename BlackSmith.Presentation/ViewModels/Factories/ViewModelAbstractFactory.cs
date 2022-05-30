using BlackSmith.Presentation.State.Navigators;
using System;

namespace BlackSmith.Presentation.ViewModels.Factories;
public class ViewModelAbstractFactory : IViewModelAbstractFactory
{
    private readonly IViewModelFactory<HomeViewModel> _homeViewModelFactory;
    private readonly IViewModelFactory<AppointmentViewModel> _appointmentViewModelFactory;

    public ViewModelAbstractFactory(IViewModelFactory<HomeViewModel> homeViewModelFactory, IViewModelFactory<AppointmentViewModel> appointmentViewModelFactory)
    {
        _homeViewModelFactory = homeViewModelFactory;
        _appointmentViewModelFactory = appointmentViewModelFactory;
    }
    public BaseViewModel CreateViewModel(ViewType viewType)
    {
        return viewType switch
        {
            ViewType.Home => _homeViewModelFactory.CreateViewModel(),
            ViewType.Appointment => _appointmentViewModelFactory.CreateViewModel(),
            _ => throw new ArgumentException("The ViewType does not have a ViewModel.", nameof(viewType))
        };
    }
}