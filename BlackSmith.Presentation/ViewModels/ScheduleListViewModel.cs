using System.Collections.Generic;
using System.Collections.ObjectModel;
using AutoMapper;
using BlackSmith.Presentation.Commands;
using BlackSmith.Presentation.Events;
using BlackSmith.Presentation.Models;
using BlackSmith.Presentation.Views.Pages;
using BlackSmith.Service.Interfaces;
using JetBrains.Annotations;
using Prism.Events;
using Prism.Mvvm;
using Wpf.Ui.Mvvm.Contracts;

namespace BlackSmith.Presentation.ViewModels;

public class ScheduleListViewModel : BindableBase
{
    private readonly IAppointmentService _appointmentService;
    private readonly IEventAggregator _eventAggregator;
    private readonly IMapper _mapper;
    private readonly INavigationService _navigationService;
    private IEnumerable<Appointment> _allAppointments = new List<Appointment>();
    private ObservableCollection<Appointment> _appointments = null!;
    private string _searchInput = "";

    public ScheduleListViewModel(IAppointmentService appointmentService,
        IMapper mapper,
        INavigationService navigationService,
        IEventAggregator eventAggregator)
    {
        _appointmentService = appointmentService;
        _mapper = mapper;
        _navigationService = navigationService;
        _eventAggregator = eventAggregator;

        GoToCreate = new RelayCommand(OnCreate);
        ClearSearchCommand = new RelayCommand(OnClearSearch);
    }

    public string SearchInput
    {
        get => _searchInput;
        set
        {
            _searchInput = value;
            RaisePropertyChanged();
        }
    }

    public RelayCommand GoToCreate { get; }
    public RelayCommand ClearSearchCommand { get; }

    public ObservableCollection<Appointment> Appointments
    {
        get => _appointments;
        set
        {
            _appointments = value;
            RaisePropertyChanged();
        }
    }

    private void OnCreate()
    {
        OnClearSearch();
        _eventAggregator.GetEvent<CreateScheduleEvent>().Publish();
        _navigationService.Navigate(typeof(ScheduleCreate));
    }

    [PublicAPI]
    public async void LoadAppointments()
    {
        var appointments = await _appointmentService.GetAppointments();
        _allAppointments = _mapper.Map<IEnumerable<Appointment>>(appointments);
        Appointments = new ObservableCollection<Appointment>(_allAppointments);
    }

    private void OnClearSearch()
    {
        SearchInput = "";
    }
}