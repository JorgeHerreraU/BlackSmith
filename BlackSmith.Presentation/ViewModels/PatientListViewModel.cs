using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using AutoMapper;
using BlackSmith.Presentation.Commands;
using BlackSmith.Presentation.Enums;
using BlackSmith.Presentation.Interfaces;
using BlackSmith.Presentation.Models;
using BlackSmith.Presentation.Services;
using BlackSmith.Service.DTOs;
using BlackSmith.Service.Interfaces;
using JetBrains.Annotations;

namespace BlackSmith.Presentation.ViewModels;

public class PatientListViewModel : BindableBase
{
    private readonly IMapper _mapper;
    private readonly IMessageService _messageService;
    private readonly INavService _navService;
    private readonly IPatientService _patientService;
    private IEnumerable<Patient> _allPatients = new List<Patient>();
    private ObservableCollection<Patient> _patients = null!;
    private string _searchInput = "";

    public PatientListViewModel(
        INavService navService,
        IMessageService messageService,
        IPatientService patientService,
        IMapper mapper)
    {
        _navService = navService;
        _messageService = messageService;
        _patientService = patientService;
        _mapper = mapper;

        LoadPatients();

        ClearSearchCommand = new RelayCommand(OnClearSearch);
        GoToCreate = new RelayCommand(OpenCreatePatient);
        EditPatientCommand = new RelayCommand<Patient>(OpenEditPatient);
        DeleteCommand = new RelayCommand<Patient>(DeletePatient);
    }

    public string SearchInput
    {
        get => _searchInput;
        set
        {
            _searchInput = value;
            NotifyPropertyChanged();
            FilterPatients(_searchInput);
        }
    }

    public ObservableCollection<Patient> Patients
    {
        get => _patients;
        set
        {
            _patients = value;
            NotifyPropertyChanged();
        }
    }

    public RelayCommand GoToCreate { get; }
    public RelayCommand ClearSearchCommand { get; }
    public RelayCommand<Patient> EditPatientCommand { get; }
    public RelayCommand<Patient> DeleteCommand { get; }

    private async void DeletePatient(Patient patient)
    {
        var confirmDeletion = await _messageService.ShowConfirmDialog("Are you sure you want to delete this patient?");
        if (!confirmDeletion) return;
        await _patientService.DeletePatient(_mapper.Map<PatientDTO>(patient));
        LoadPatients();
    }

    private void OpenEditPatient(Patient patient)
    {
        _navService.Navigate(new NavigationTriggeredEventArgs { Page = Pages.PatientEdit, Model = patient });
    }

    private void OpenCreatePatient()
    {
        OnClearSearch();
        _navService.Navigate(new NavigationTriggeredEventArgs { Page = Pages.PatientCreate });
    }

    [PublicAPI]
    public async void LoadPatients()
    {
        var patients = await _patientService.GetPatients();
        _allPatients = _mapper.Map<IEnumerable<Patient>>(patients);
        Patients = new ObservableCollection<Patient>(_allPatients);
    }

    private void FilterPatients(string searchInput)
    {
        var isSearchInputNull = string.IsNullOrWhiteSpace(searchInput);
        var appointments = new ObservableCollection<Patient>(_allPatients);
        var filteredAppointments = new ObservableCollection<Patient>(
            _allPatients.ToList().Where(c =>
                c.FullName.ToLower().Contains(searchInput.ToLower())
            ));

        Patients = isSearchInputNull ? appointments : filteredAppointments;
    }

    private void OnClearSearch()
    {
        SearchInput = "";
    }
}