using AutoMapper;
using BlackSmith.Core.Helpers;
using BlackSmith.Core.Structs;
using BlackSmith.Presentation.Enums;
using BlackSmith.Presentation.Extensions;
using BlackSmith.Presentation.Interfaces;
using BlackSmith.Presentation.Models;
using BlackSmith.Service.DTOs;
using BlackSmith.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Wpf.Ui.Mvvm.Contracts;

namespace BlackSmith.Presentation.ViewModels.Schedules;

public class ScheduleEditViewModel : ScheduleBaseViewModel
{
    private readonly IAppointmentService _appointmentService;
    private readonly IDoctorService _doctorService;
    private readonly IMapper _mapper;
    private readonly IPatientService _patientService;
    private IEnumerable<DoctorDTO> _allDoctors = new List<DoctorDTO>();
    private IEnumerable<Patient> _allPatients = new List<Patient>();
    private Appointment _appointment = null!;
    private ObservableCollection<DateTime> _blackOutDates = new();
    private ObservableCollection<Doctor> _doctors = new();
    private ObservableCollection<Patient> _patients = new();
    private DateTime? _selectedDate;
    private Speciality? _selectedSpeciality = Speciality.GeneralPractice;
    private TimeOnly _selectedStartTime;

    public ScheduleEditViewModel(
        IModalService modalService,
        IPatientService patientService,
        IMapper mapper,
        INavigationService navigationService,
        IDoctorService doctorService,
        IAppointmentService appointmentService) : base(modalService, navigationService)
    {
        _patientService = patientService;
        _mapper = mapper;
        _doctorService = doctorService;
        _appointmentService = appointmentService;
    }

    public Appointment Appointment
    {
        get => _appointment;
        set
        {
            _appointment = value;
            Initialize();
            NotifyPropertyChanged();
        }
    }

    public ObservableCollection<DateTime> BlackoutDates
    {
        get => _blackOutDates;
        set
        {
            _blackOutDates = value;
            NotifyPropertyChanged();
        }
    }

    public DateTime EndingDate { get; set; } = DateHelper.GenerateDatesUntilNextMonth();

    public ObservableCollection<Doctor> Doctors
    {
        get => _doctors;
        private set
        {
            _doctors = value;
            NotifyPropertyChanged();
        }
    }
    public Patient? SelectedPatient
    {
        get => Appointment.Patient;
        set
        {
            Appointment.Patient = value;
            if (value != null) Appointment.PatientId = value.Id;
            NotifyPropertyChanged();
        }
    }

    [Required(ErrorMessage = "The Speciality field is required")]
    public Speciality? SelectedSpeciality
    {
        get => _selectedSpeciality;
        set
        {
            _selectedSpeciality = value;
            NotifyPropertyChanged();
            FilterDates();
            SaveCommand.RaiseCanExecuteChanged();
        }
    }

    [Required(ErrorMessage = "The Date field is required")]
    public DateTime? SelectedDate
    {
        get => _selectedDate;
        set
        {
            _selectedDate = value;
            NotifyPropertyChanged();
            FilterDoctors();
            FilterTimes();
        }
    }

    public ObservableCollection<Patient> Patients
    {
        get => _patients;
        private set
        {
            _patients = value;
            NotifyPropertyChanged();
        }
    }

    [Required(ErrorMessage = "The Doctor field is required")]
    public Doctor? SelectedDoctor
    {
        get => Appointment.Doctor;
        set
        {
            Appointment.Doctor = value;
            if (value != null) Appointment.DoctorId = value.Id;
            NotifyPropertyChanged();
            FilterTimes();
        }
    }

    public TimeOnly SelectedStartTime
    {
        get => _selectedStartTime;
        set
        {
            _selectedStartTime = value;
            if (SelectedDate != null)
            {
                Appointment.Start = SelectedDate.Value.Date + _selectedStartTime.ToTimeSpan();
                Appointment.End = Appointment.Start.Value.AddMinutes(Appointment.Duration);
            }
            NotifyPropertyChanged();
        }
    }

    public override async void Initialize()
    {
        await LoadCollectionsData();
        ResetAvailableHours();
        SubscribeChanges();
        SetDefaultValues();
    }

    private async Task LoadCollectionsData()
    {
        _allDoctors = await _doctorService.GetDoctors();
        _allPatients = _mapper.Map<List<Patient>>(await _patientService.GetPatients());
        Doctors = new ObservableCollection<Doctor>(_mapper.Map<IEnumerable<Doctor>>(_allDoctors));
        Patients = new ObservableCollection<Patient>(_allPatients);
    }

    protected override void SubscribeChanges()
    {
        Appointment.PropertyChanged += OnPropertyChanged;
        Appointment.ErrorsChanged += RaiseCanChange;
    }

    private void SetDefaultValues()
    {
        IsTouched = true;
        SelectedSpeciality = Doctors.FirstOrDefault(d => d.Id == Appointment.DoctorId)?.Speciality;
        SelectedPatient = Patients.FirstOrDefault(p => p.Id == Appointment.PatientId);
        SelectedDate = Appointment.Start;
        SelectedStartTime = TimeOnly.FromDateTime(Appointment.Start!.Value);
    }

    protected override bool CanSave()
    {
        return !Appointment.HasErrors && SelectedSpeciality != null;
    }

    protected override async void OnSave()
    {
        try
        {
            await _appointmentService.UpdateAppointment(_mapper.Map<AppointmentDTO>(Appointment));
            OnGoBack();
        }
        catch (Exception ex)
        {
            OnSaveErrorHandler(ex);
        }
    }

    private async void FilterDoctors()
    {
        if (SelectedDate == null || SelectedSpeciality == null)
        {
            Doctors = new ObservableCollection<Doctor>();
            return;
        }
        var speciality = SelectedSpeciality.Value.ToDTO();
        var dateRange = new DateRange(DateTime.Now, EndingDate);
        var selectedDate = SelectedDate.Value;
        var doctors = await _appointmentService.FilterAvailableDoctors(
            speciality,
            dateRange,
            selectedDate,
            _allDoctors);
        Doctors = new ObservableCollection<Doctor>(_mapper.Map<IEnumerable<Doctor>>(doctors));
        SelectedDoctor = Doctors.FirstOrDefault(d => d.Id == Appointment.DoctorId);
    }

    private async void FilterDates()
    {
        if (SelectedSpeciality == null) return;
        ClearDates();
        var speciality = SelectedSpeciality.Value.ToDTO();
        var dateRange = new DateRange(DateTime.Now, EndingDate);
        var days = dateRange.Dates;
        var blackoutDays = await _appointmentService.GetSpecialityBlackoutDays(
            speciality,
            dateRange,
            days);
        foreach (var blackoutDay in blackoutDays) BlackoutDates.Add(blackoutDay);
    }

    private async void FilterTimes()
    {
        if (SelectedPatient == null || SelectedDate == null || SelectedDoctor == null || SelectedDoctor.Id == 0)
            return;
        var selectedDoctor = _mapper.Map<DoctorDTO>(SelectedDoctor);
        var selectedDate = SelectedDate.Value;
        var availableHours = await _appointmentService.GetAvailableHoursByDoctor(selectedDoctor, selectedDate);
        AvailableHours = new ObservableCollection<TimeOnly>(availableHours);
        if (Appointment.Start == null) return;
        var patient = _mapper.Map<PatientDTO>(Appointment.Patient);
        var appointmentDate = Appointment.Start.Value;
        var appointment = await _appointmentService.GetAppointmentByPatientAndDate(patient, appointmentDate);
        var hourIsMissing = CheckAppointmentHourIsMissing(availableHours, appointment, appointmentDate);
        if (hourIsMissing) AddAppointmentTimeToAvailableHours(availableHours, appointmentDate);
        SelectedStartTime = TimeOnly.FromDateTime(appointmentDate);
    }

    private void ClearDates()
    {
        SelectedDate = null;
        BlackoutDates.Clear();
    }


    private bool CheckAppointmentHourIsMissing(ICollection<TimeOnly> availableHours,
        AppointmentDTO? appointment,
        DateTime appointmentDate)
    {
        var hourNotExists = !availableHours.Contains(TimeOnly.FromDateTime(appointmentDate));
        var isPatientAlreadyScheduled = appointment?.PatientId == Appointment.Patient?.Id;
        return hourNotExists && isPatientAlreadyScheduled;
    }

    private void AddAppointmentTimeToAvailableHours(List<TimeOnly> availableHours, DateTime selectedDate)
    {
        availableHours.Add(TimeOnly.FromDateTime(selectedDate));
        availableHours.Sort();
        AvailableHours = new ObservableCollection<TimeOnly>(availableHours);
    }

    public override void Dispose()
    {
        Appointment.PropertyChanged -= OnPropertyChanged;
        Appointment.ErrorsChanged -= RaiseCanChange;
    }
}
