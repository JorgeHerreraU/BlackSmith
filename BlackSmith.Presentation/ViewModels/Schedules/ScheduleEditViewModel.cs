using AutoMapper;
using BlackSmith.Core.Helpers;
using BlackSmith.Presentation.Enums;
using BlackSmith.Presentation.Filters;
using BlackSmith.Presentation.Interfaces;
using BlackSmith.Presentation.Models;
using BlackSmith.Presentation.Views.Pages.Schedules;
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
    private readonly DateTimeFilter _dateTimeFilter;
    private readonly IDoctorService _doctorService;
    private readonly IMapper _mapper;
    private readonly INavigationService _navigationService;
    private readonly IPatientService _patientService;
    private List<Doctor> _allDoctors = new();
    private List<Patient> _allPatients = new();
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
        IAppointmentService appointmentService,
        DoctorFilter doctorFilter,
        DateTimeFilter dateTimeFilter) : base(modalService, appointmentService, mapper, doctorFilter)
    {
        _patientService = patientService;
        _mapper = mapper;
        _navigationService = navigationService;
        _doctorService = doctorService;
        _appointmentService = appointmentService;
        _dateTimeFilter = dateTimeFilter;
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

    private void SetDefaultValues()
    {
        IsTouched = true;
        SelectedSpeciality = Doctors.FirstOrDefault(d => d.Id == Appointment.DoctorId)?.Speciality;
        SelectedPatient = Patients.FirstOrDefault(p => p.Id == Appointment.PatientId);
        SelectedDate = Appointment.Start;
        SelectedStartTime = TimeOnly.FromDateTime(Appointment.Start!.Value);
    }

    protected override void SubscribeChanges()
    {
        Appointment.PropertyChanged += OnPropertyChanged;
        Appointment.ErrorsChanged += RaiseCanChange;
    }

    protected override bool CanSave()
    {
        return !Appointment.HasErrors && SelectedSpeciality != null;
    }

    protected override void OnGoBack()
    {
        _navigationService.Navigate(typeof(ScheduleList));
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
        var availableDays = await GetSpecialityAvailableDays(SelectedSpeciality.Value, DateTime.Now, EndingDate);
        var doctors = GetFilteredDoctors(SelectedSpeciality.Value, SelectedDate.Value, availableDays, _allDoctors);
        Doctors = new ObservableCollection<Doctor>(doctors);
        SelectedDoctor = Doctors.FirstOrDefault(d => d.Id == Appointment.DoctorId);
    }

    private async void FilterDates()
    {
        if (SelectedSpeciality == null) return;
        ClearDates();
        var days = DateHelper.GetDateRange(DateTime.Now, EndingDate).ToList();
        var availableDays = await GetSpecialityAvailableDays(SelectedSpeciality.Value, DateTime.Now, EndingDate);
        var specification = new DateTimeNotContainsSpecification(availableDays);
        var filteredDates = _dateTimeFilter.Filter(days, specification);
        foreach (var blackoutDay in filteredDates) BlackoutDates.Add(blackoutDay);
    }

    private void ClearDates()
    {
        SelectedDate = null;
        BlackoutDates.Clear();
    }

    private async Task LoadCollectionsData()
    {
        _allDoctors = _mapper.Map<List<Doctor>>(await _doctorService.GetDoctors());
        _allPatients = _mapper.Map<List<Patient>>(await _patientService.GetPatients());
        Doctors = new ObservableCollection<Doctor>(_allDoctors);
        Patients = new ObservableCollection<Patient>(_allPatients);
    }

    private async void FilterTimes()
    {
        if (SelectedPatient == null || SelectedDate == null || SelectedDoctor == null || SelectedDoctor.Id == 0)
            return;
        var availableHours = await GetDoctorAvailableHours(SelectedDoctor, SelectedDate.Value);
        AvailableHours = new ObservableCollection<TimeOnly>(availableHours);
        if (Appointment.Start == null) return;
        var appointment = await GetAppointmentByPatientAndDate();
        var hourIsMissing = CheckSelectedHourIsMissing(availableHours, appointment);
        if (hourIsMissing) AddSelectedTimeToAvailableHours(availableHours);
        SelectedStartTime = TimeOnly.FromDateTime(Appointment.Start.Value);
    }

    private bool CheckSelectedHourIsMissing(ICollection<TimeOnly> availableHours,
        AppointmentDTO? appointment)
    {
        var hourNotExists = !availableHours.Contains(TimeOnly.FromDateTime(Appointment.Start!.Value));
        var isPatientAlreadyScheduled = appointment?.PatientId == Appointment.Patient?.Id;
        return hourNotExists && isPatientAlreadyScheduled;
    }

    private void AddSelectedTimeToAvailableHours(List<TimeOnly> availableHours)
    {
        availableHours.Add(TimeOnly.FromDateTime(Appointment.Start!.Value));
        availableHours.Sort();
        AvailableHours = new ObservableCollection<TimeOnly>(availableHours);
    }

    private async Task<AppointmentDTO?> GetAppointmentByPatientAndDate()
    {
        return await _appointmentService.GetAppointmentByPatientAndDate(
            _mapper.Map<PatientDTO>(Appointment.Patient),
            Appointment.Start!.Value);
    }

    public override void Dispose()
    {
        Appointment.PropertyChanged -= OnPropertyChanged;
        Appointment.ErrorsChanged -= RaiseCanChange;
    }
}
