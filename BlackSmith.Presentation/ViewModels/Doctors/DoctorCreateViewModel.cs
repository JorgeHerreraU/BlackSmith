using AutoMapper;
using BlackSmith.Core.Helpers;
using BlackSmith.Presentation.Helpers;
using BlackSmith.Presentation.Interfaces;
using BlackSmith.Presentation.Models;
using BlackSmith.Presentation.Views.Pages.Doctors;
using BlackSmith.Service.DTOs;
using BlackSmith.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Wpf.Ui.Common.Interfaces;
using Wpf.Ui.Mvvm.Contracts;

namespace BlackSmith.Presentation.ViewModels.Doctors;

public class DoctorCreateViewModel : EditableViewModelBase, INavigationAware
{
    private readonly IDoctorService _doctorService;
    private readonly IMapper _mapper;
    private readonly INavigationService _navigationService;
    private ObservableCollection<WorkingDay> _availableWorkingDays = new();
    private Doctor _doctor = null!;

    public DoctorCreateViewModel(
        IDoctorService doctorService,
        IMapper mapper,
        IModalService modalService,
        INavigationService navigationService
    ) : base(modalService)
    {
        _doctorService = doctorService;
        _mapper = mapper;
        _navigationService = navigationService;
    }
    public List<TimeOnly> AvailableHours { get; } =
        TimeHelper.GetTimeRange(Appointment.StartingHour, Appointment.EndingHour).ToList();

    public ObservableCollection<WorkingDay> AvailableWorkingDays
    {
        get => _availableWorkingDays;
        private set
        {
            _availableWorkingDays = value;
            RaisePropertyChanged();
        }
    }

    public Doctor Doctor
    {
        get => _doctor;
        private set
        {
            _doctor = value;
            RaisePropertyChanged();
        }
    }

    public void OnNavigatedTo() { }

    public void OnNavigatedFrom()
    {
        Dispose();
    }

    public override void Initialize()
    {
        Doctor = new Doctor();
        AvailableWorkingDays = new ObservableCollection<WorkingDay>(
            WorkingDayHelper.GetDefaultWorkingDays()
        );
        SubscribeChanges();
        IsTouched = false;
    }

    protected override bool CanSave()
    {
        return !Doctor.HasErrors && !Doctor.Address.HasErrors;
    }

    protected override async void OnSave()
    {
        try
        {
            Doctor.WorkingDays = AvailableWorkingDays.Where(w => w.IsChecked is true).ToList();
            await _doctorService.CreateDoctor(_mapper.Map<DoctorDTO>(Doctor));
            OnGoBack();
        }
        catch (Exception ex)
        {
            OnSaveErrorHandler(ex);
        }
    }

    protected override void OnGoBack()
    {
        _navigationService.Navigate(typeof(DoctorList));
    }

    public override void SubscribeChanges()
    {
        Doctor.PropertyChanged += OnPropertyChanged;
        Doctor.Address.PropertyChanged += OnPropertyChanged;
        Doctor.ErrorsChanged += RaiseCanChange;
        Doctor.Address.ErrorsChanged += RaiseCanChange;
    }

    public override void Dispose()
    {
        Doctor.PropertyChanged -= OnPropertyChanged;
        Doctor.Address.PropertyChanged -= OnPropertyChanged;
        Doctor.ErrorsChanged -= RaiseCanChange;
        Doctor.Address.ErrorsChanged -= RaiseCanChange;
    }
}
