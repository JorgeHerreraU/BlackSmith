using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using AutoMapper;
using BlackSmith.Presentation.Commands;
using BlackSmith.Presentation.Enums;
using BlackSmith.Presentation.Helpers;
using BlackSmith.Presentation.Interfaces;
using BlackSmith.Presentation.Models;
using BlackSmith.Presentation.Services;
using BlackSmith.Service.DTOs;
using BlackSmith.Service.Interfaces;
using FluentValidation;

namespace BlackSmith.Presentation.ViewModels;

public class DoctorCreateViewModel : BindableBase
{
    private readonly IDoctorService _doctorService;
    private readonly IMapper _mapper;
    private readonly IMessageService _messageService;
    private readonly INavService _navService;
    private Doctor _doctor = null!;
    private bool _isTouched;

    public DoctorCreateViewModel(INavService navService, IDoctorService doctorService, IMapper mapper,
        IMessageService messageService)
    {
        _navService = navService;
        _doctorService = doctorService;
        _mapper = mapper;
        _messageService = messageService;

        SaveCommand = new RelayCommand(OnSave, CanSave);
        GoBack = new RelayCommand(OnGoBack);

        SetNewDoctor();
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

    public List<WorkingDay> AvailableWorkingDays { get; } = new()
    {
        new WorkingDay
        {
            Day = DayOfWeek.Monday,
            StartTime = TimeOnly.MinValue,
            EndTime = TimeOnly.MaxValue,
            IsChecked = false
        },
        new WorkingDay
        {
            Day = DayOfWeek.Tuesday,
            StartTime = TimeOnly.MinValue,
            EndTime = TimeOnly.MaxValue,
            IsChecked = false
        },
        new WorkingDay
        {
            Day = DayOfWeek.Wednesday,
            StartTime = TimeOnly.MinValue,
            EndTime = TimeOnly.MaxValue,
            IsChecked = false
        },
        new WorkingDay
        {
            Day = DayOfWeek.Thursday,
            StartTime = TimeOnly.MinValue,
            EndTime = TimeOnly.MaxValue,
            IsChecked = false
        },
        new WorkingDay
        {
            Day = DayOfWeek.Friday,
            StartTime = TimeOnly.MinValue,
            EndTime = TimeOnly.MaxValue,
            IsChecked = false
        }
    };

    public RelayCommand GoBack { get; }
    public RelayCommand SaveCommand { get; }

    public Doctor Doctor
    {
        get => _doctor;
        private set
        {
            _doctor = value;
            NotifyPropertyChanged();
        }
    }

    public bool IsTouched
    {
        get => _isTouched;
        set
        {
            _isTouched = value;
            NotifyPropertyChanged();
        }
    }

    private void SetNewDoctor()
    {
        Doctor = new Doctor();
        Doctor.PropertyChanged += OnDoctorPropertyChanged;
        Doctor.Address.PropertyChanged += OnDoctorPropertyChanged;
        Doctor.ErrorsChanged += RaiseCanChange;
        Doctor.Address.ErrorsChanged += RaiseCanChange;
        IsTouched = false;
    }

    private void RaiseCanChange(object? sender, DataErrorsChangedEventArgs e)
    {
        SaveCommand.RaiseCanExecuteChanged();
    }

    private void OnDoctorPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (sender != null)
            IsTouched = StringHelper.IsAnyStringNullOrEmpty(sender) is false;
    }

    private bool CanSave()
    {
        return !Doctor.HasErrors && !Doctor.Address.HasErrors;
    }

    private void OnClear()
    {
        SetNewDoctor();
    }

    private async void OnSave()
    {
        try
        {
            Doctor.WorkingDays = AvailableWorkingDays.Where(w => w.IsChecked is true).ToList();
            await _doctorService.CreateDoctor(_mapper.Map<DoctorDTO>(Doctor));
            OnClear();
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
                _messageService.ShowErrorMessage(StringHelper.SanitizeFluentErrorMessage(ex.Message));
                break;
            case ArgumentException:
                _messageService.ShowErrorMessage(ex.Message);
                break;
        }
    }

    private void OnGoBack()
    {
        _navService.Navigate(new NavigationTriggeredEventArgs { Page = Pages.DoctorList });
    }
}