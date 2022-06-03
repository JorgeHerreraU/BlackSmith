using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using BlackSmith.Presentation.Commands;
using BlackSmith.Service.DTOs;
using BlackSmith.Service.Interfaces;

namespace BlackSmith.Presentation.Modules.Patients;

public class PatientListViewModel : BindableBase
{
    private readonly IPatientService _patientService;
    private IEnumerable<PatientDTO> _patients = new List<PatientDTO>();
    private ObservableCollection<PatientDTO> _patientsObs = new();
    private string _searchInput = "";

    public PatientListViewModel(IPatientService patientService)
    {
        _patientService = patientService;
        LoadPatients();
        ClearSearchCommand = new RelayCommand(OnClearSearch);
    }

    public string SearchInput
    {
        get => _searchInput;
        set
        {
            SetPropertyChanged(ref _searchInput, value);
            FilterPatients(_searchInput);
        }
    }

    public ObservableCollection<PatientDTO>? Patients
    {
        get => _patientsObs;
        private set => SetPropertyChanged(ref _patientsObs!, value);
    }

    public RelayCommand ClearSearchCommand { get; }

    private async void LoadPatients()
    {
        _patients = await _patientService.GetPatients();
        Patients = new ObservableCollection<PatientDTO>(_patients);
    }

    private void FilterPatients(string searchInput)
    {
        var isSearchInputNull = string.IsNullOrWhiteSpace(searchInput);
        var appointments = new ObservableCollection<PatientDTO>(_patients);
        var filteredAppointments = new ObservableCollection<PatientDTO>(
            _patients.ToList().Where(c =>
                c.FullName.ToLower().Contains(searchInput.ToLower())
            ));

        Patients = isSearchInputNull ? appointments : filteredAppointments;
    }

    private void OnClearSearch()
    {
        SearchInput = "";
    }
}