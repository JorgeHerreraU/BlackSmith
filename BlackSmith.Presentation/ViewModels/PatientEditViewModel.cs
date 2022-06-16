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

namespace BlackSmith.Presentation.ViewModels;

public class PatientEditViewModel : BindableBase
{
    private readonly IMapper _mapper;
    private readonly IMessageService _messageService;
    private readonly INavService _navService;
    private readonly IPatientService _patientService;
    private bool _isTouched;
    private Patient _patient = new();

    public PatientEditViewModel(
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
    }

    public Patient Patient
    {
        get => _patient;
        set
        {
            _patient = value;
            SetPatient();
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

    public void SetPatient()
    {
        Patient.PropertyChanged += OnPatientPropertyChanged;
        Patient.Address.PropertyChanged += OnPatientPropertyChanged;
        Patient.ErrorsChanged += RaiseCanChange;
        Patient.Address.ErrorsChanged += RaiseCanChange;
        IsTouched = false;
    }

    private void RaiseCanChange(object? sender, DataErrorsChangedEventArgs e)
    {
        SaveCommand.RaiseCanExecuteChanged();
    }

    private void OnPatientPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (sender != null)
            IsTouched = StringHelper.IsAnyStringNullOrEmpty(sender) is false;
    }

    private void OnGoBack()
    {
        _navService.Navigate(new NavigationTriggeredEventArgs { Page = Pages.PatientList });
    }

    private bool CanSave()
    {
        return !Patient.HasErrors && !Patient.Address.HasErrors;
    }

    private async void OnSave()
    {
        try
        {
            await _patientService.UpdatePatient(_mapper.Map<PatientDTO>(Patient));
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
}