using System;
using System.ComponentModel;
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

public class PatientEditViewModel : BindableBase
{
    private readonly IMapper _mapper;
    private readonly IModalService _modalService;
    private readonly INavigationService _navigationService;
    private readonly IPatientService _patientService;
    private bool _isTouched;
    private Patient _patient = new();

    public PatientEditViewModel(IMapper mapper,
        IPatientService patientService,
        IModalService modalService,
        INavigationService navigationService)
    {
        _mapper = mapper;
        _patientService = patientService;
        _modalService = modalService;
        _navigationService = navigationService;

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

    public RelayCommand SaveCommand { get; }

    public RelayCommand GoBack { get; }

    private void SetPatient()
    {
        Patient.PropertyChanged += OnPatientPropertyChanged;
        Patient.Address.PropertyChanged += OnPatientPropertyChanged;
        Patient.ErrorsChanged += RaiseCanChange;
        Patient.Address.ErrorsChanged += RaiseCanChange;
        IsTouched = false;
    }

    private void RaiseCanChange(object? sender,
        DataErrorsChangedEventArgs e)
    {
        SaveCommand.RaiseCanExecuteChanged();
    }

    private void OnPatientPropertyChanged(object? sender,
        PropertyChangedEventArgs e)
    {
        if (sender != null)
            IsTouched = StringHelper.IsAnyStringNullOrEmpty(sender) is false;
    }

    private void OnGoBack()
    {
        _navigationService.Navigate(typeof(PatientList));
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
                _modalService.ShowErrorMessage(StringHelper.SanitizeFluentErrorMessage(ex.Message));
                break;
            case ArgumentException:
                _modalService.ShowErrorMessage(ex.Message);
                break;
        }
    }
}