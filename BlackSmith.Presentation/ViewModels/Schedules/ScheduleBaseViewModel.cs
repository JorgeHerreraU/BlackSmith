using BlackSmith.Core.Structs;
using BlackSmith.Presentation.Interfaces;
using BlackSmith.Presentation.Models;
using BlackSmith.Presentation.Views.Pages.Schedules;
using System;
using System.Collections.ObjectModel;
using Wpf.Ui.Common.Interfaces;
using Wpf.Ui.Mvvm.Contracts;

namespace BlackSmith.Presentation.ViewModels.Schedules;

public abstract class ScheduleBaseViewModel : EditableViewModelBase, INavigationAware
{
    private readonly INavigationService _navigationService;
    private ObservableCollection<TimeOnly> _availableHours = new();
    protected ScheduleBaseViewModel(IModalService modalService,
        INavigationService navigationService) : base(modalService)
    {
        _navigationService = navigationService;
    }

    public ObservableCollection<TimeOnly> AvailableHours
    {
        get => _availableHours;
        protected set
        {
            _availableHours = value;
            NotifyPropertyChanged();
        }
    }

    public void OnNavigatedTo() { }

    public void OnNavigatedFrom()
    {
        Dispose();
    }

    protected void ResetAvailableHours()
    {
        var timeRange = new TimeRange(Appointment.StartingHour, Appointment.EndingHour).Times;
        AvailableHours = new ObservableCollection<TimeOnly>(timeRange);
    }

    protected override void OnGoBack()
    {
        _navigationService.Navigate(typeof(ScheduleList));
    }
}
