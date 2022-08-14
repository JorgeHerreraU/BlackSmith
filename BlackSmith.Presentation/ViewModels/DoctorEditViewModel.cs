using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using AutoMapper;
using BlackSmith.Presentation.Commands;
using BlackSmith.Presentation.Helpers;
using BlackSmith.Presentation.Interfaces;
using BlackSmith.Presentation.Models;
using BlackSmith.Presentation.Views.Pages;
using BlackSmith.Service.DTOs;
using BlackSmith.Service.Interfaces;
using FluentValidation;
using Prism.Mvvm;
using Wpf.Ui.Mvvm.Contracts;

namespace BlackSmith.Presentation.ViewModels;

public class DoctorEditViewModel : BindableBase
{
    private readonly IDoctorService _doctorService;
    private readonly IMapper _mapper;
    private readonly IModalService _modalService;
    private readonly INavigationService _navigationService;

    private BindingList<WorkingDay> _availableWorkingDays = new()
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

    private Doctor _doctor = null!;
    private bool _isTouched;

    public DoctorEditViewModel(IMapper mapper,
        IDoctorService doctorService,
        IModalService modalService,
        INavigationService navigationService)
    {
        _mapper = mapper;
        _doctorService = doctorService;
        _modalService = modalService;
        _navigationService = navigationService;

        SaveCommand = new RelayCommand(OnSave, CanSave);
        GoBack = new RelayCommand(OnGoBack);
    }

    public RelayCommand SaveCommand { get; }
    public RelayCommand GoBack { get; }

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

    public BindingList<WorkingDay> AvailableWorkingDays
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
        set
        {
            _doctor = value;
            SetDoctor(value);
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

    private void OnGoBack()
    {
        _navigationService.Navigate(typeof(DoctorList));
    }

    private bool CanSave()
    {
        return !Doctor.HasErrors && !Doctor.Address.HasErrors;
    }

    private async void OnSave()
    {
        try
        {
            Doctor.WorkingDays.Clear();
            Doctor.WorkingDays = AvailableWorkingDays.Where(w => w.IsChecked is true).ToList();
            await _doctorService.UpdateDoctor(_mapper.Map<DoctorDTO>(Doctor));
            OnGoBack();
        }
        catch (Exception e)
        {
            OnSaveErrorHandler(e);
        }
    }

    private void SetDoctor(Doctor doctor)
    {
        var workingDays = AvailableWorkingDays.ToList();
        workingDays.ForEach(day =>
        {
            day.IsChecked = doctor.WorkingDays.Any(d => d.Day == day.Day);
            day.StartTime = doctor.WorkingDays.FirstOrDefault(d => d.Day == day.Day)?.StartTime ?? TimeOnly.MinValue;
            day.EndTime = doctor.WorkingDays.FirstOrDefault(d => d.Day == day.Day)?.EndTime ?? TimeOnly.MaxValue;
        });
        AvailableWorkingDays = new BindingList<WorkingDay>(workingDays);
        Doctor.PropertyChanged += OnDoctorPropertyChanged;
        Doctor.Address.PropertyChanged += OnDoctorPropertyChanged;
        Doctor.ErrorsChanged += RaiseCanChange;
        Doctor.Address.ErrorsChanged += RaiseCanChange;
        AvailableWorkingDays.ListChanged += RaiseCanChange;
        IsTouched = false;
    }

    private void RaiseCanChange(object? sender,
        ListChangedEventArgs e)
    {
        if (sender is null) return;
        IsTouched = StringHelper.IsAnyStringNullOrEmpty(Doctor) is false;
        SaveCommand.RaiseCanExecuteChanged();
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
        SaveCommand.RaiseCanExecuteChanged();
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
}