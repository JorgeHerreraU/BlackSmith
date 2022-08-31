using AutoMapper;
using BlackSmith.Presentation.Extensions;
using BlackSmith.Presentation.Interfaces;
using BlackSmith.Presentation.Models;
using BlackSmith.Presentation.Views.Pages;
using BlackSmith.Service.DTOs;
using BlackSmith.Service.Interfaces;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Wpf.Ui.Mvvm.Contracts;

namespace BlackSmith.Presentation.ViewModels;

public class DoctorEditViewModel : EditableViewModelBase
{
    private readonly IDoctorService _doctorService;
    private readonly IMapper _mapper;
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

    public DoctorEditViewModel(IMapper mapper,
        IDoctorService doctorService,
        IModalService modalService,
        INavigationService navigationService) : base(modalService)
    {
        _mapper = mapper;
        _doctorService = doctorService;
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
            Set();
            RaisePropertyChanged();
        }
    }

    protected override void OnGoBack()
    {
        _navigationService.Navigate(typeof(DoctorList));
    }

    protected override bool CanSave()
    {
        return !Doctor.HasErrors && !Doctor.Address.HasErrors;
    }

    protected override async void OnSave()
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

    private void RaiseCanChange(object? sender, ListChangedEventArgs e)
    {
        if (sender is null)
            return;
        IsTouched = Doctor.IsAnyStringNullOrEmpty();
        SaveCommand.RaiseCanExecuteChanged();
    }
    public override void Set()
    {
        var workingDays = AvailableWorkingDays.ToList();
        workingDays.ForEach(day =>
        {
            day.IsChecked = Doctor.WorkingDays.Any(d => d.Day == day.Day);
            day.StartTime = Doctor.WorkingDays.FirstOrDefault(d => d.Day == day.Day)?.StartTime ?? TimeOnly.MinValue;
            day.EndTime = Doctor.WorkingDays.FirstOrDefault(d => d.Day == day.Day)?.EndTime ?? TimeOnly.MaxValue;
        });
        AvailableWorkingDays = new BindingList<WorkingDay>(workingDays);
        SubscribeChanges();
        IsTouched = false;
    }

    public override void SubscribeChanges()
    {
        Doctor.PropertyChanged += OnPropertyChanged;
        Doctor.Address.PropertyChanged += OnPropertyChanged;
        Doctor.ErrorsChanged += RaiseCanChange;
        Doctor.Address.ErrorsChanged += RaiseCanChange;
        AvailableWorkingDays.ListChanged += RaiseCanChange;
    }
}