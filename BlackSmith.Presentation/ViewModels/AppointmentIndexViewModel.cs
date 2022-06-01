using System.Collections.ObjectModel;
using BlackSmith.Domain.Interfaces;
using BlackSmith.Domain.Models;

namespace BlackSmith.Presentation.ViewModels;

public class AppointmentIndexViewModel : BaseViewModel
{
    private readonly IAppointmentService _appointmentService;

    public AppointmentIndexViewModel(IAppointmentService appointmentService)
    {
        _appointmentService = appointmentService;
        Initialize();
    }

    public ObservableCollection<Appointment>? Appointments { get; set; }

    private async void Initialize()
    {
        Appointments = new ObservableCollection<Appointment>(await _appointmentService.GetAppointments());
    }
}