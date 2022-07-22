using System.Collections.Generic;
using System.Collections.ObjectModel;
using AutoMapper;
using BlackSmith.Presentation.Commands;
using BlackSmith.Presentation.Interfaces;
using BlackSmith.Presentation.Models;
using BlackSmith.Presentation.Services;
using BlackSmith.Presentation.Views.Pages;
using BlackSmith.Service.Interfaces;
using JetBrains.Annotations;
using Prism.Mvvm;

namespace BlackSmith.Presentation.ViewModels;

public class ScheduleListViewModel : BindableBase
{
    private readonly IAppointmentService _appointmentService;
    private readonly IMapper _mapper;
    private readonly INavService _navService;
    private IEnumerable<Appointment> _allAppointments = new List<Appointment>();
    private ObservableCollection<Appointment> _appointments = null!;
    private string _searchInput = "";

    public ScheduleListViewModel(INavService navService,
        IAppointmentService appointmentService,
        IMapper mapper)
    {
        _navService = navService;
        _appointmentService = appointmentService;
        _mapper = mapper;

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
        _navService.Navigate(new NavigationTriggeredEventArgs { Page = typeof(ScheduleCreate) });
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