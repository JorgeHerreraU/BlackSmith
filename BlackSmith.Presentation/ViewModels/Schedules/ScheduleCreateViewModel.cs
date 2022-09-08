using AutoMapper;
using BlackSmith.Core.Helpers;
using BlackSmith.Core.ValidationAttributes;
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

public class ScheduleCreateViewModel : ScheduleBaseViewModel
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
    private ObservableCollection<Doctor> _doctors = null!;
    private ObservableCollection<Patient> _patients = new();
    private Speciality? _selectedSpeciality = Speciality.GeneralPractice;
    private TimeOnly _selectedStartTime;

    public ScheduleCreateViewModel(
        IAppointmentService appointmentService,
        IModalService modalService,
        IMapper mapper,
        INavigationService navigationService,
        IDoctorService doctorService,
        IPatientService patientService,
        DoctorFilter doctorFilter,
        DateTimeFilter dateTimeFilter) : base(modalService, appointmentService, mapper, doctorFilter)
    {
        _appointmentService = appointmentService;
        _navigationService = navigationService;
        _doctorService = doctorService;
        _patientService = patientService;
        _dateTimeFilter = dateTimeFilter;
        _mapper = mapper;
    }

    private Appointment Appointment
    {
        get => _appointment;
        set
        {
            _appointment = value;
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

    public ObservableCollection<Patient> Patients
    {
        get => _patients;
        private set
        {
            _patients = value;
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
    [DateGreaterThanToday]
    public DateTime? SelectedDate
    {
        get => Appointment.Start;
        set
        {
            Appointment.Start = value;
            NotifyPropertyChanged();
            FilterDoctors();
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

    [Required(ErrorMessage = "The Patient field is required")]
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

    private async void FilterTimes()
    {
        if (SelectedDate == null || SelectedDoctor == null || SelectedDoctor.Id == 0) return;
        var availableHours = await GetDoctorAvailableHours(SelectedDoctor, SelectedDate.Value);
        AvailableHours = new ObservableCollection<TimeOnly>(availableHours);
        SelectedStartTime = AvailableHours.First();
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
    }

    protected override void OnGoBack()
    {
        _navigationService.Navigate(typeof(ScheduleList));
    }

    protected override bool CanSave()
    {
        return !Appointment.HasErrors && SelectedSpeciality != null;
    }

    protected override async void OnSave()
    {
        try
        {
            await _appointmentService.CreateAppointment(_mapper.Map<AppointmentDTO>(Appointment));
            OnGoBack();
        }
        catch (Exception ex)
        {
            OnSaveErrorHandler(ex);
        }
    }

    public override async void Initialize()
    {
        Appointment = new Appointment();
        await LoadCollectionsData();
        ResetAvailableHours();
        SubscribeChanges();
        SetDefaultValues();
    }

    private void SetDefaultValues()
    {
        IsTouched = false;
        SelectedSpeciality = Speciality.GeneralPractice;
        SelectedPatient = null!;
        SelectedDoctor = null!;
        SelectedDate = null!;
        SelectedStartTime = AvailableHours.First();
    }

    private async Task LoadCollectionsData()
    {
        _allDoctors = _mapper.Map<List<Doctor>>(await _doctorService.GetDoctors());
        _allPatients = _mapper.Map<List<Patient>>(await _patientService.GetPatients());
        Patients = new ObservableCollection<Patient>(_allPatients);
    }

    protected override void SubscribeChanges()
    {
        Appointment.PropertyChanged += OnPropertyChanged;
        Appointment.ErrorsChanged += RaiseCanChange;
    }

    public override void Dispose()
    {
        Appointment.PropertyChanged -= OnPropertyChanged;
        Appointment.ErrorsChanged -= RaiseCanChange;
    }
}
