using BlackSmith.Presentation.Models;
using BlackSmith.Presentation.Views.Pages;
using Prism.Commands;
using Prism.Mvvm;
using Wpf.Ui.Mvvm.Contracts;

namespace BlackSmith.Presentation.ViewModels;

public class DoctorDetailViewModel : BindableBase
{
    private readonly INavigationService _navigationService;
    private Doctor _doctor = null!;

    public DoctorDetailViewModel(INavigationService navigationService)
    {
        _navigationService = navigationService;
        GoBack = new DelegateCommand(OnGoBack);
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

    public DelegateCommand GoBack { get; }

    private void OnGoBack()
    {
        _navigationService.Navigate(typeof(DoctorList));
    }
}