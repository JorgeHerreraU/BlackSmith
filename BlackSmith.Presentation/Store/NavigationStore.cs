using System;
using BlackSmith.Presentation.Modules.Appointments;
using BlackSmith.Presentation.Modules.Patients;

namespace BlackSmith.Presentation.Store;

public class NavigationStore
{
    private readonly AppointmentListViewModel _appointmentListViewModel;
    private readonly PatientListViewModel _patientListViewModel;
    private BindableBase? _selectedAppointmentViewModel;
    private BindableBase? _selectedPatientViewModel;
    private BindableBase? _selectedViewModel;

    public NavigationStore(
        AppointmentListViewModel appointmentListViewModel,
        PatientListViewModel patientListViewModel
    )
    {
        _appointmentListViewModel = appointmentListViewModel;
        _patientListViewModel = patientListViewModel;
    }

    public BindableBase SelectedViewModel
    {
        get => _selectedViewModel!;
        set
        {
            _selectedViewModel?.Dispose();
            _selectedViewModel = value;
            SelectedViewModelChanged?.Invoke();
        }
    }

    public BindableBase SelectedAppointmentViewModel
    {
        get => _selectedAppointmentViewModel ?? _appointmentListViewModel;
        set
        {
            _selectedAppointmentViewModel?.Dispose();
            _selectedAppointmentViewModel = value;
            SelectedAppointmentViewModelChanged?.Invoke();
        }
    }

    public BindableBase SelectedPatientViewModel
    {
        get => _selectedPatientViewModel ?? _patientListViewModel;
        set
        {
            _selectedPatientViewModel?.Dispose();
            _selectedPatientViewModel = value;
            SelectedPatientViewModelChanged?.Invoke();
        }
    }

    public event Action? SelectedViewModelChanged;
    public event Action? SelectedAppointmentViewModelChanged;
    public event Action? SelectedPatientViewModelChanged;
}