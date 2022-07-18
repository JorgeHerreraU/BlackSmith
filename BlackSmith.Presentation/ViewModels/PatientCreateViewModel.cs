using System;
using System.ComponentModel;
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
using JetBrains.Annotations;
using Wpf.Ui.Mvvm;

namespace BlackSmith.Presentation.ViewModels;

public class PatientCreateViewModel : ViewModelBase
{
    private readonly IMapper _mapper;
    private readonly IModalService _modalService;
    private readonly INavService _navService;
    private readonly IPatientService _patientService;
    private bool _isTouched;
    private Patient _patient = null!;

    public PatientCreateViewModel(INavService navService,
        IMapper mapper,
        IPatientService patientService,
        IModalService modalService)
    {
        _navService = navService;
        _mapper = mapper;
        _patientService = patientService;
        _modalService = modalService;

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
            SetValue(value);
        }
    }

    public bool IsTouched
    {
        get => _isTouched;
        set
        {
            _isTouched = value;
            SetValue(value);
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

    private void OnPatientPropertyChanged(object? sender,
        PropertyChangedEventArgs e)
    {
        if (sender != null)
            IsTouched = StringHelper.IsAnyStringNullOrEmpty(sender) is false;
    }


    [PublicAPI]
    public void RaiseCanChange(object? sender,
        EventArgs e)
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
                _modalService.ShowErrorMessage(StringHelper.SanitizeFluentErrorMessage(ex.Message));
                break;
            case ArgumentException:
                _modalService.ShowErrorMessage(ex.Message);
                break;
        }
    }

    private void OnGoBack()
    {
        _navService.Navigate(new NavigationTriggeredEventArgs { Page = typeof(PatientList) });
    }
}