using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using BlackSmith.Presentation.Commands;
using BlackSmith.Service.DTOs;
using BlackSmith.Service.Interfaces;

namespace BlackSmith.Presentation.Modules.Appointments;

public class AppointmentListViewModel : BindableBase
{
    private readonly IAppointmentService _appointmentService;
    private IEnumerable<AppointmentDTO> _appointments = new List<AppointmentDTO>();
    private ObservableCollection<AppointmentDTO> _appointmentsObs = new();
    private string _searchInput = "";

    public AppointmentListViewModel(IAppointmentService appointmentService)
    {
        _appointmentService = appointmentService;
        LoadAppointments();
        ClearSearchCommand = new RelayCommand(OnClearSearch);
    }

    public string SearchInput
    {
        get => _searchInput;
        set
        {
            SetPropertyChanged(ref _searchInput, value);
            FilterAppointments(_searchInput);
        }
    }

    public ObservableCollection<AppointmentDTO>? Appointments
    {
        get => _appointmentsObs;
        private set => SetPropertyChanged(ref _appointmentsObs!, value);
    }

    public RelayCommand ClearSearchCommand { get; }


    private async void LoadAppointments()
    {
        _appointments = await _appointmentService.GetAppointments();
        Appointments = new ObservableCollection<AppointmentDTO>(_appointments);
    }

    private void FilterAppointments(string searchInput)
    {
        var isSearchInputNull = string.IsNullOrWhiteSpace(searchInput);
        var appointments = new ObservableCollection<AppointmentDTO>(_appointments);
        var filteredAppointments = new ObservableCollection<AppointmentDTO>(
            _appointments.ToList().Where(c =>
                c.Doctor.FullName.ToLower().Contains(searchInput.ToLower()) ||
                c.Patient.FullName.ToLower().Contains(searchInput.ToLower())
            ));

        Appointments = isSearchInputNull ? appointments : filteredAppointments;
    }

    private void OnClearSearch()
    {
        SearchInput = "";
    }
}