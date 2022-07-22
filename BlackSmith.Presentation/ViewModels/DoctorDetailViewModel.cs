using BlackSmith.Presentation.Commands;
using BlackSmith.Presentation.Interfaces;
using BlackSmith.Presentation.Models;
using BlackSmith.Presentation.Services;
using BlackSmith.Presentation.Views.Pages;
using Prism.Mvvm;

namespace BlackSmith.Presentation.ViewModels;

public class DoctorDetailViewModel : BindableBase
{
    private readonly INavService _navService;
    private Doctor _doctor = null!;

    public DoctorDetailViewModel(INavService navService)
    {
        _navService = navService;
        GoBack = new RelayCommand(OnGoBack);
    }

    public Doctor Doctor
    {
        get => _doctor;
        set
        {
            _doctor = value;
            RaisePropertyChanged();
        }
    }

    public RelayCommand GoBack { get; }

    private void OnGoBack()
    {
        _navService.Navigate(new NavigationTriggeredEventArgs { Page = typeof(DoctorList) });
    }
}