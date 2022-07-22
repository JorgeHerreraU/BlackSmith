using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using AutoMapper;
using BlackSmith.Presentation.Commands;
using BlackSmith.Presentation.Helpers;
using BlackSmith.Presentation.Interfaces;
using BlackSmith.Presentation.Models;
using BlackSmith.Presentation.Services;
using BlackSmith.Presentation.Views.Pages;
using BlackSmith.Service.DTOs;
using BlackSmith.Service.Interfaces;
using FluentValidation;
using Prism.Mvvm;

namespace BlackSmith.Presentation.ViewModels;

public class DoctorCreateViewModel : BindableBase
{
    private readonly IDoctorService _doctorService;
    private readonly IMapper _mapper;
    private readonly IModalService _modalService;
    private readonly INavService _navService;
    private ObservableCollection<WorkingDay> _availableWorkingDays = new();
    private Doctor _doctor = null!;
    private bool _isTouched;

    public DoctorCreateViewModel(INavService navService,
        IDoctorService doctorService,
        IMapper mapper,
        IModalService modalService)
    {
        _navService = navService;
        _doctorService = doctorService;
        _mapper = mapper;
        _modalService = modalService;

        SaveCommand = new RelayCommand(OnSave, CanSave);
        GoBack = new RelayCommand(OnGoBack);
        ClearCommand = new RelayCommand(OnClear);
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

    public RelayCommand GoBack { get; }
    public RelayCommand SaveCommand { get; }
    public RelayCommand ClearCommand { get; }

    public Doctor Doctor
    {
        get => _doctor;
        private set
        {
            _doctor = value;
            RaisePropertyChanged();
        }
    }

    public bool IsTouched
    {
        get => _isTouched;
        set
        {
            _isTouched = value;
            RaisePropertyChanged();
        }
    }

    private void OnClear()
    {
        SetNewDoctor();
    }

    public void SetNewDoctor()
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
        Doctor.PropertyChanged += OnDoctorPropertyChanged;
        Doctor.Address.PropertyChanged += OnDoctorPropertyChanged;
        Doctor.ErrorsChanged += RaiseCanChange;
        Doctor.Address.ErrorsChanged += RaiseCanChange;
        IsTouched = false;
    }

    private void RaiseCanChange(object? sender,
        DataErrorsChangedEventArgs e)
    {
        SaveCommand.RaiseCanExecuteChanged();
    }

    private void OnDoctorPropertyChanged(object? sender,
        PropertyChangedEventArgs e)
    {
        if (sender != null)
            IsTouched = StringHelper.IsAnyStringNullOrEmpty(sender) is false;
    }

    private bool CanSave()
    {
        return !Doctor.HasErrors && !Doctor.Address.HasErrors;
    }

    private async void OnSave()
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

    private void OnSaveErrorHandler(Exception ex)
    {
        switch (ex)
        {
            case ValidationException:
                _modalService.ShowErrorMessage(StringHelper.SanitizeFluentErrorMessage(ex.Message));
                break;
            case ArgumentException:
                _modalService.ShowErrorMessage(ex.Message);
                break;
        }
    }

    private void OnGoBack()
    {
        _navService.Navigate(new NavigationTriggeredEventArgs { Page = typeof(DoctorList) });
    }
}