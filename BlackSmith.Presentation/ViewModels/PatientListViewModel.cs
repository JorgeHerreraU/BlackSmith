using AutoMapper;
using BlackSmith.Presentation.Events;
using BlackSmith.Presentation.Interfaces;
using BlackSmith.Presentation.Models;
using BlackSmith.Presentation.Views.Pages;
using BlackSmith.Service.DTOs;
using BlackSmith.Service.Interfaces;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Wpf.Ui.Mvvm.Contracts;

namespace BlackSmith.Presentation.ViewModels;

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

    public PatientListViewModel(IModalService modalService,
        IPatientService patientService,
        IMapper mapper,
        IEventAggregator eventAggregator,
        INavigationService navigationService)
    {
        _modalService = modalService;
        _patientService = patientService;
        _mapper = mapper;
        _eventAggregator = eventAggregator;
        _navigationService = navigationService;

        LoadPatients();

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
            FilterPatients(_searchInput);
        }
    }

    public ObservableCollection<Patient> Patients
    {
        get => _patients;
        set
        {
            _patients = value;
            RaisePropertyChanged();
        }
    }

    public DelegateCommand GoToCreate { get; }
    public DelegateCommand ClearSearchCommand
    {
        get;
    }
    public DelegateCommand<Patient> EditPatientCommand { get; }
    public DelegateCommand<Patient> DeleteCommand { get; }

    private async void OnDelete(Patient patient)
    {
        var confirmDeletion = await _modalService.ShowConfirmDialog("Are you sure you want to delete this patient?");
        if (!confirmDeletion)
            return;
        await _patientService.DeletePatient(_mapper.Map<PatientDTO>(patient));
        LoadPatients();
    }

    private void OnEdit(Patient patient)
    {
        _eventAggregator.GetEvent<EditPatientEvent>().Publish(patient);
        _navigationService.Navigate(typeof(PatientEdit));
    }

    private void OnCreate()
    {
        OnClearSearch();
        _eventAggregator.GetEvent<CreatePatientEvent>().Publish();
        _navigationService.Navigate(typeof(PatientCreate));

    }

    public async void LoadPatients()
    {
        var patients = await _patientService.GetPatients();
        _allPatients = _mapper.Map<IEnumerable<Patient>>(patients);
        Patients = new ObservableCollection<Patient>(_allPatients);
    }

    private void FilterPatients(string searchInput)
    {
        var isSearchInputNull = string.IsNullOrWhiteSpace(searchInput);
        var patients = new ObservableCollection<Patient>(_allPatients);
        var filteredResults = new ObservableCollection<Patient>(_allPatients.ToList()
            .Where(c => c.FullName.ToLower().Contains(searchInput.ToLower())));

        Patients = isSearchInputNull ? patients : filteredResults;
    }

    private void OnClearSearch()
    {
        SearchInput = "";
    }
}