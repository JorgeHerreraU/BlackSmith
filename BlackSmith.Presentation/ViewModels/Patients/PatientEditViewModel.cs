using AutoMapper;
using BlackSmith.Presentation.Interfaces;
using BlackSmith.Presentation.Models;
using BlackSmith.Presentation.Views.Pages.Patients;
using BlackSmith.Service.DTOs;
using BlackSmith.Service.Interfaces;
using System;
using Wpf.Ui.Common.Interfaces;
using Wpf.Ui.Mvvm.Contracts;

namespace BlackSmith.Presentation.ViewModels.Patients;

public class PatientEditViewModel : EditableViewModelBase, INavigationAware
{
    private readonly IMapper _mapper;
    private readonly INavigationService _navigationService;
    private readonly IPatientService _patientService;
    private Patient _patient = new();

    public PatientEditViewModel(
        IMapper mapper,
        IPatientService patientService,
        IModalService modalService,
        INavigationService navigationService
    ) : base(modalService)
    {
        _mapper = mapper;
        _patientService = patientService;
        _navigationService = navigationService;
    }

    public Patient Patient
    {
        get => _patient;
        set
        {
            _patient = value;
            Initialize();
            RaisePropertyChanged();
        }
    }

    protected override void OnGoBack()
    {
        _navigationService.Navigate(typeof(PatientList));
    }

    protected override bool CanSave()
    {
        return !Patient.HasErrors && !Patient.Address.HasErrors;
    }

    protected override async void OnSave()
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

    public override void Initialize()
    {
        SubscribeChanges();
        IsTouched = true;
    }

    public override void SubscribeChanges()
    {
        Patient.PropertyChanged += OnPropertyChanged;
        Patient.Address.PropertyChanged += OnPropertyChanged;
        Patient.ErrorsChanged += RaiseCanChange;
        Patient.Address.ErrorsChanged += RaiseCanChange;
    }

    public override void Dispose()
    {
        Patient.PropertyChanged -= OnPropertyChanged;
        Patient.Address.PropertyChanged -= OnPropertyChanged;
        Patient.ErrorsChanged -= RaiseCanChange;
        Patient.Address.ErrorsChanged -= RaiseCanChange;
    }

    public void OnNavigatedTo() { }

    public void OnNavigatedFrom()
    {
        Dispose();
    }
}
