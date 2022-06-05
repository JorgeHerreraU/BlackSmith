using System;
using AutoMapper;
using BlackSmith.Presentation.Commands;
using BlackSmith.Service.DTOs;
using BlackSmith.Service.Interfaces;

namespace BlackSmith.Presentation.Modules.Appointments;

public class AppointmentCreateEditViewModel : BindableBase
{
    private readonly IAppointmentService _appointmentService;
    private readonly IMapper _mapper;
    private SimpleEditableAppointment _appointment = new();

    private AppointmentDTO _editingAppointment = null!;
    private bool _editMode;

    public AppointmentCreateEditViewModel(IAppointmentService appointmentService, IMapper mapper)
    {
        _appointmentService = appointmentService;
        _mapper = mapper;
        CancelCommand = new RelayCommand(OnCancel);
        SaveCommand = new RelayCommand(OnSave, CanSave);
    }

    public bool EditMode
    {
        get => _editMode;
        set => SetPropertyChanged(ref _editMode, value);
    }

    public RelayCommand CancelCommand { get; }
    public RelayCommand SaveCommand { get; }

    public SimpleEditableAppointment Appointment
    {
        get => _appointment;
        set => SetPropertyChanged(ref _appointment, value);
    }

    private bool CanSave()
    {
        return !Appointment.HasErrors;
    }

    private async void OnSave()
    {
        _mapper.Map(Appointment, _editingAppointment);
        if (EditMode)
            await _appointmentService.UpdateAppointment(_editingAppointment);
        else
            await _appointmentService.AddAppointment(_editingAppointment);
        Done();
    }

    public event Action Done = delegate { };

    public void SetAppointment(AppointmentDTO appointment)
    {
        try
        {
            _editingAppointment = appointment;
            Appointment = new SimpleEditableAppointment();
            _mapper.Map(appointment, Appointment);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

        // _mapper.Map(Appointment, appointment);
        // if (Appointment != null) Appointment.ErrorsChanged -= RaiseCanExecuteChanged;
        // Appointment = new SimpleEditableAppointment();
        // Appointment.ErrorsChanged += RaiseCanExecuteChanged;
        // _mapper.Map(Appointment, _editingAppointment);
    }

    private void RaiseCanExecuteChanged(object sender, EventArgs e)
    {
        SaveCommand.RaiseCanExecuteChanged();
    }


    private void OnCancel()
    {
        Done();
    }
}