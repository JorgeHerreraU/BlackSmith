using AutoMapper;
using BlackSmith.Core.ExtensionMethods;
using BlackSmith.Core.Structs;
using BlackSmith.Presentation.Helpers;
using BlackSmith.Presentation.Interfaces;
using BlackSmith.Presentation.Models;
using BlackSmith.Presentation.Views.Pages.Doctors;
using BlackSmith.Service.DTOs;
using BlackSmith.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Wpf.Ui.Common.Interfaces;
using Wpf.Ui.Mvvm.Contracts;

namespace BlackSmith.Presentation.ViewModels.Doctors;

public class DoctorEditViewModel : EditableViewModelBase, INavigationAware
{
    private readonly IDoctorService _doctorService;
    private readonly IMapper _mapper;
    private readonly INavigationService _navigationService;
    private BindingList<WorkingDay> _availableWorkingDays =
        new(WorkingDayHelper.GetDefaultWorkingDays().ToList());
    private Doctor _doctor = null!;

    public DoctorEditViewModel(
        IMapper mapper,
        IDoctorService doctorService,
        IModalService modalService,
        INavigationService navigationService
    ) : base(modalService)
    {
        _mapper = mapper;
        _doctorService = doctorService;
        _navigationService = navigationService;
    }

    public List<TimeOnly> AvailableHours { get; } =
        new TimeRange(Appointment.StartingHour, Appointment.EndingHour).Times.ToList();
    public BindingList<WorkingDay> AvailableWorkingDays
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
        set
        {
            _doctor = value;
            Initialize();
            RaisePropertyChanged();
        }
    }

    public void OnNavigatedTo() { }

    public void OnNavigatedFrom()
    {
        Dispose();
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

    protected override void SubscribeChanges()
    {
        Doctor.PropertyChanged += OnPropertyChanged;
        Doctor.Address.PropertyChanged += OnPropertyChanged;
        Doctor.ErrorsChanged += RaiseCanChange;
        Doctor.Address.ErrorsChanged += RaiseCanChange;
        AvailableWorkingDays.ListChanged += RaiseCanChange;
    }

    public override void Initialize()
    {
        var workingDays = AvailableWorkingDays.ToList();
        SetDoctorsWorkingDays(workingDays);
        AvailableWorkingDays = new BindingList<WorkingDay>(workingDays);
        SubscribeChanges();
        IsTouched = true;
    }

    private void RaiseCanChange(object? sender, ListChangedEventArgs e)
    {
        if (sender is null) return;
        IsTouched = !Doctor.CheckAnyStringNullOrEmpty();
        SaveCommand.RaiseCanExecuteChanged();
    }

    private void SetDoctorsWorkingDays(List<WorkingDay> workingDays)
    {
        workingDays.ForEach(day =>
        {
            day.IsChecked = Doctor.WorkingDays.Any(d => d.Day == day.Day);
            day.StartTime =
                Doctor.WorkingDays.FirstOrDefault(d => d.Day == day.Day)?.StartTime
                ?? TimeOnly.MinValue;
            day.EndTime =
                Doctor.WorkingDays.FirstOrDefault(d => d.Day == day.Day)?.EndTime
                ?? TimeOnly.MaxValue;
        });
    }

    public override void Dispose()
    {
        Doctor.PropertyChanged -= OnPropertyChanged;
        Doctor.Address.PropertyChanged -= OnPropertyChanged;
        Doctor.ErrorsChanged -= RaiseCanChange;
        Doctor.Address.ErrorsChanged -= RaiseCanChange;
        AvailableWorkingDays.ListChanged -= RaiseCanChange;
    }
}
