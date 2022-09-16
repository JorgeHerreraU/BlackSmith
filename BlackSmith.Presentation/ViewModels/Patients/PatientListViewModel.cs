using AutoMapper;
using BlackSmith.Core;
using BlackSmith.Presentation.Events;
using BlackSmith.Presentation.Interfaces;
using BlackSmith.Presentation.Models;
using BlackSmith.Presentation.Views.Pages.Patients;
using BlackSmith.Service.DTOs;
using BlackSmith.Service.Interfaces;
using JetBrains.Annotations;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Wpf.Ui.Mvvm.Contracts;

namespace BlackSmith.Presentation.ViewModels.Patients;

public class PatientListViewModel : BindableBase
{
    private readonly IEventAggregator _eventAggregator;
    private readonly IMapper _mapper;
    private readonly IModalService _modalService;
    private readonly INavigationService _navigationService;
    private readonly IPatientService _patientService;
    private IEnumerable<Patient> _allPatients = new List<Patient>();
    private ObservableCollection<Patient> _patients = null!;
    private string _searchInput = "";

    public PatientListViewModel(
        IModalService modalService,
        IPatientService patientService,
        IMapper mapper,
        IEventAggregator eventAggregator,
        INavigationService navigationService
    )
    {
        _modalService = modalService;
        _patientService = patientService;
        _mapper = mapper;
        _eventAggregator = eventAggregator;
        _navigationService = navigationService;

        ClearSearchCommand = new DelegateCommand(OnClearSearch);
        GoToCreate = new DelegateCommand(OnCreate);
        EditPatientCommand = new DelegateCommand<Patient>(OnEdit);
        DeleteCommand = new DelegateCommand<Patient>(OnDelete);
    }

    public string SearchInput
    {
        get => _searchInput;
        set
        {
            _searchInput = value;
            RaisePropertyChanged();
            FilterData(_searchInput);
        }
    }

    public ObservableCollection<Patient> Patients
    {
        get => _patients;
        private set
        {
            _patients = value;
            RaisePropertyChanged();
        }
    }

    public DelegateCommand GoToCreate { get; }
    public DelegateCommand ClearSearchCommand { get; }
    public DelegateCommand<Patient> EditPatientCommand { get; }
    public DelegateCommand<Patient> DeleteCommand { get; }

    private async void OnDelete(Patient patient)
    {
        var confirmDeletion = await _modalService.ShowConfirmDialog(Constants.Messages.ConfirmDeletion, patient);
        if (!confirmDeletion) return;
        await _patientService.DeletePatient(_mapper.Map<PatientDTO>(patient));
        Load();
    }

    private void OnEdit(Patient patient)
    {
        _navigationService.Navigate(typeof(PatientEdit));
        _eventAggregator.GetEvent<EditPatientEvent>().Publish(patient);
    }

    private void OnCreate()
    {
        OnClearSearch();
        _navigationService.Navigate(typeof(PatientCreate));
        _eventAggregator.GetEvent<CreatePatientEvent>().Publish();
    }

    [PublicAPI]
    public async void Load()
    {
        var patients = await _patientService.GetPatients();
        _allPatients = _mapper.Map<IEnumerable<Patient>>(patients);
        Patients = new ObservableCollection<Patient>(_allPatients);
    }

    private void FilterData(string searchInput)
    {
        var isSearchInputNull = string.IsNullOrWhiteSpace(searchInput);
        var patients = new ObservableCollection<Patient>(_allPatients);
        var filteredResults = new ObservableCollection<Patient>(_allPatients.Where(HasPatientName(searchInput)));
        Patients = isSearchInputNull ? patients : filteredResults;
    }

    private static Func<Patient, bool> HasPatientName(string searchInput)
    {
        return c => c.FullName.ToLower().Contains(searchInput.ToLower());
    }

    private void OnClearSearch()
    {
        SearchInput = "";
    }
}
