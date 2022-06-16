using System;
using System.ComponentModel;
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
using JetBrains.Annotations;

namespace BlackSmith.Presentation.ViewModels;

public class PatientCreateViewModel : BindableBase
{
    private readonly IMapper _mapper;
    private readonly IMessageService _messageService;
    private readonly INavService _navService;
    private readonly IPatientService _patientService;
    private bool _isTouched;
    private Patient _patient = null!;

    public PatientCreateViewModel(
        INavService navService,
        IMapper mapper,
        IPatientService patientService,
        IMessageService messageService
    )
    {
        _navService = navService;
        _mapper = mapper;
        _patientService = patientService;
        _messageService = messageService;

        SaveCommand = new RelayCommand(OnSave, CanSave);
        GoBack = new RelayCommand(OnGoBack);
        ClearCommand = new RelayCommand(OnClear);

        SetNewPatient();
    }

    public Patient Patient
    {
        get => _patient;
        private set
        {
            _patient = value;
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

    public RelayCommand SaveCommand { get; }

    public RelayCommand GoBack { get; }

    public RelayCommand ClearCommand { get; }

    private void OnClear()
    {
        SetNewPatient();
    }

    [PublicAPI]
    public void SetNewPatient()
    {
        Patient = new Patient();
        Patient.PropertyChanged += OnPatientPropertyChanged;
        Patient.Address.PropertyChanged += OnPatientPropertyChanged;
        Patient.ErrorsChanged += RaiseCanChange;
        Patient.Address.ErrorsChanged += RaiseCanChange;
        IsTouched = false;
    }

    private void OnPatientPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (sender != null)
            IsTouched = StringHelper.IsAnyStringNullOrEmpty(sender) is false;
    }


    [PublicAPI]
    public void RaiseCanChange(object? sender, EventArgs e)
    {
        SaveCommand.RaiseCanExecuteChanged();
    }

    private bool CanSave()
    {
        return !Patient.HasErrors && !Patient.Address.HasErrors;
    }

    private async void OnSave()
    {
        try
        {
            await _patientService.CreatePatient(_mapper.Map<PatientDTO>(Patient));
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
        _navService.Navigate(new NavigationTriggeredEventArgs { Page = Pages.PatientList });
    }
}