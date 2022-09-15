using AutoMapper;
using BlackSmith.Core.Helpers;
using BlackSmith.Presentation.Events;
using BlackSmith.Presentation.Interfaces;
using BlackSmith.Presentation.Models;
using BlackSmith.Presentation.Views.Pages.Schedules;
using BlackSmith.Service.DTOs;
using BlackSmith.Service.Interfaces;
using JetBrains.Annotations;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using Wpf.Ui.Mvvm.Contracts;

namespace BlackSmith.Presentation.ViewModels.Schedules;

public class ScheduleListViewModel : BindableBase
{
    private readonly IAppointmentService _appointmentService;
    private readonly IEventAggregator _eventAggregator;
    private readonly IMapper _mapper;
    private readonly IModalService _modalService;
    private readonly INavigationService _navigationService;
    private IEnumerable<Appointment> _allAppointments = new List<Appointment>();
    private ObservableCollection<Appointment> _appointments = null!;
    private string _searchInput = "";

    public ScheduleListViewModel(
        IAppointmentService appointmentService,
        IMapper mapper,
        INavigationService navigationService,
        IEventAggregator eventAggregator,
        IModalService modalService
    )
    {
        _appointmentService = appointmentService;
        _mapper = mapper;
        _navigationService = navigationService;
        _eventAggregator = eventAggregator;
        _modalService = modalService;

        GoToCreate = new DelegateCommand(OnCreate);
        ClearSearchCommand = new DelegateCommand(OnClearSearch);
        EditCommand = new DelegateCommand<Appointment>(OnEdit, CanEdit);
        DeleteCommand = new DelegateCommand<Appointment>(OnDelete);
        ConfirmAppointmentCommand = new DelegateCommand<Appointment>(OnConfirmAppointment, CanConfirm);
    }

    public string SearchInput
    {
        get => _searchInput;
        set
        {
            _searchInput = value;
            RaisePropertyChanged();
            FilterData(_searchInput);
        }
    }

    public DelegateCommand GoToCreate { get; }
    public DelegateCommand ClearSearchCommand { get; }
    public DelegateCommand<Appointment> EditCommand { get; }
    public DelegateCommand<Appointment> DeleteCommand { get; }
    public DelegateCommand<Appointment> ConfirmAppointmentCommand { get; }

    public ObservableCollection<Appointment> Appointments
    {
        get => _appointments;
        private set
        {
            _appointments = value;
            RaisePropertyChanged();
        }
    }

    [PublicAPI]
    public async void Load()
    {
        var appointments = await _appointmentService.GetAppointments();
        _allAppointments = _mapper.Map<IEnumerable<Appointment>>(appointments).ToList().OrderBy(a => a.Start);
        Appointments = new ObservableCollection<Appointment>(_allAppointments);
    }
    private static bool CanConfirm(Appointment appointment)
    {
        return appointment.Start > DateTime.Now;
    }

    private static bool CanEdit(Appointment appointment)
    {
        return appointment.Start > DateTime.Now;
    }

    private async void OnDelete(Appointment appointment)
    {
        var confirmDeletion = await _modalService.ShowConfirmDialog(
            "Are you sure you want to delete this appointment?"
        );
        if (!confirmDeletion) return;
        await _appointmentService.DeleteAppointment(_mapper.Map<AppointmentDTO>(appointment));
        Load();
    }

    private void OnEdit(Appointment appointment)
    {
        _navigationService.Navigate(typeof(ScheduleEdit));
        _eventAggregator.GetEvent<EditScheduleEvent>().Publish(appointment);
    }

    private void OnCreate()
    {
        OnClearSearch();
        _navigationService.Navigate(typeof(ScheduleCreate));
        _eventAggregator.GetEvent<CreateScheduleEvent>().Publish();
    }

    private async void OnConfirmAppointment(Appointment appointment)
    {
        appointment.IsConfirmed ^= true;
        await _appointmentService.UpdateAppointment(_mapper.Map<AppointmentDTO>(appointment));
    }

    private void OnClearSearch()
    {
        SearchInput = "";
    }

    private void FilterData(string searchInput)
    {
        var isSearchInputNull = string.IsNullOrWhiteSpace(searchInput);
        var appointments = new ObservableCollection<Appointment>(_allAppointments);
        var predicate = PredicateBuilder.False<Appointment>();
        predicate = predicate
            .Or(HasPatientName(searchInput))
            .Or(HasDoctorName(searchInput))
            .Or(HasPatientIdentification(searchInput));
        var filteredResults = new ObservableCollection<Appointment>(
            _allAppointments.ToList().Where(predicate.Compile())
        );
        Appointments = isSearchInputNull ? appointments : filteredResults;
    }

    private static Expression<Func<Appointment, bool>> HasPatientName(string searchInput)
    {
        return a =>
            a.Patient!.FullName.ToLower().Contains(searchInput.ToLower());
    }

    private static Expression<Func<Appointment, bool>> HasDoctorName(string searchInput)
    {
        return a =>
            a.Doctor!.FullName.ToLower().Contains(searchInput.ToLower());
    }

    private static Expression<Func<Appointment, bool>> HasPatientIdentification(string searchInput)
    {
        return a => a.Patient!.Identification.ToLower().Contains(searchInput.ToLower());
    }
}
