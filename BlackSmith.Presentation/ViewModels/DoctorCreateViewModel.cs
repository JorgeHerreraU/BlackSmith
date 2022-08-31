using AutoMapper;
using BlackSmith.Presentation.Interfaces;
using BlackSmith.Presentation.Models;
using BlackSmith.Presentation.Views.Pages;
using BlackSmith.Service.DTOs;
using BlackSmith.Service.Interfaces;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Wpf.Ui.Mvvm.Contracts;

namespace BlackSmith.Presentation.ViewModels;

public class DoctorCreateViewModel : EditableViewModelBase
{
    private readonly IDoctorService _doctorService;
    private readonly IMapper _mapper;

    private readonly INavigationService _navigationService;
    private ObservableCollection<WorkingDay> _availableWorkingDays = new();

    private Doctor _doctor = null!;

    public DoctorCreateViewModel(IDoctorService doctorService,
        IMapper mapper,
        IModalService modalService,
        INavigationService navigationService) : base(modalService)
    {
        _doctorService = doctorService;
        _mapper = mapper;
        _navigationService = navigationService;
    }

    public List<TimeOnly> AvailableHours { get; } = new()
    {
        new TimeOnly(9, 0, 0),
        new TimeOnly(10, 0, 0),
        new TimeOnly(11, 0, 0),
        new TimeOnly(12, 0, 0),
        new TimeOnly(13, 0, 0),
        new TimeOnly(14, 0, 0),
        new TimeOnly(15, 0, 0),
        new TimeOnly(16, 0, 0),
        new TimeOnly(17, 0, 0),
        new TimeOnly(18, 0, 0)
    };

    public ObservableCollection<WorkingDay> AvailableWorkingDays
    {
        get => _availableWorkingDays;
        set
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

    public override void Set()
    {
        Doctor = new Doctor();
        AvailableWorkingDays = new ObservableCollection<WorkingDay>
        {
            new()
            {
                Day = DayOfWeek.Monday,
                StartTime = TimeOnly.MinValue,
                EndTime = TimeOnly.MaxValue,
                IsChecked = false
            },
            new()
            {
                Day = DayOfWeek.Tuesday,
                StartTime = TimeOnly.MinValue,
                EndTime = TimeOnly.MaxValue,
                IsChecked = false
            },
            new()
            {
                Day = DayOfWeek.Wednesday,
                StartTime = TimeOnly.MinValue,
                EndTime = TimeOnly.MaxValue,
                IsChecked = false
            },
            new()
            {
                Day = DayOfWeek.Thursday,
                StartTime = TimeOnly.MinValue,
                EndTime = TimeOnly.MaxValue,
                IsChecked = false
            },
            new()
            {
                Day = DayOfWeek.Friday,
                StartTime = TimeOnly.MinValue,
                EndTime = TimeOnly.MaxValue,
                IsChecked = false
            }
        };
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
}
