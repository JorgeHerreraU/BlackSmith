using AutoMapper;
using BlackSmith.Core.Helpers;
using BlackSmith.Core.Structs;
using BlackSmith.Core.ValidationAttributes;
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

public class ScheduleCreateViewModel : ScheduleBaseViewModel
{
    private readonly IAppointmentService _appointmentService;
    private readonly IDoctorService _doctorService;
    private readonly IMapper _mapper;
    private readonly IPatientService _patientService;
    private IEnumerable<DoctorDTO> _allDoctors = new List<DoctorDTO>();
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
        IPatientService patientService) : base(modalService, navigationService)
    {
        _appointmentService = appointmentService;
        _doctorService = doctorService;
        _patientService = patientService;
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
                Appointment.Start = DateHelper.CombineDateAndTime(SelectedDate.Value, _selectedStartTime);
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

    public override async void Initialize()
    {
        Appointment = new Appointment();
        await LoadCollectionsData();
        ResetAvailableHours();
        SubscribeChanges();
        SetDefaultValues();
    }

    private async Task LoadCollectionsData()
    {
        _allDoctors = await _doctorService.GetDoctors();
        _allPatients = _mapper.Map<List<Patient>>(await _patientService.GetPatients());
        Patients = new ObservableCollection<Patient>(_allPatients);
    }

    protected override void SubscribeChanges()
    {
        Appointment.PropertyChanged += OnPropertyChanged;
        Appointment.ErrorsChanged += RaiseCanChange;
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
        if (SelectedDate == null || SelectedDoctor == null || SelectedDoctor.Id == 0) return;
        var selectedDoctor = _mapper.Map<DoctorDTO>(SelectedDoctor);
        var selectedDate = SelectedDate.Value;
        var availableHours = await _appointmentService.GetAvailableHoursByDoctor(selectedDoctor, selectedDate);
        AvailableHours = new ObservableCollection<TimeOnly>(availableHours);
        SelectedStartTime = AvailableHours.First();
    }

    private void ClearDates()
    {
        SelectedDate = null;
        BlackoutDates.Clear();
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
    public override void Dispose()
    {
        Appointment.PropertyChanged -= OnPropertyChanged;
        Appointment.ErrorsChanged -= RaiseCanChange;
    }
}
