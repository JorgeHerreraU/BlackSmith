using AutoMapper;
using BlackSmith.Core.ExtensionMethods;
using BlackSmith.Core.Helpers;
using BlackSmith.Core.Structs;
using BlackSmith.Core.ValidationAttributes;
using BlackSmith.Presentation.Enums;
using BlackSmith.Presentation.Extensions;
using BlackSmith.Presentation.Interfaces;
using BlackSmith.Presentation.Models;
using BlackSmith.Presentation.Views.Pages.Schedules;
using BlackSmith.Service.DTOs;
using BlackSmith.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Wpf.Ui.Common.Interfaces;
using Wpf.Ui.Mvvm.Contracts;

namespace BlackSmith.Presentation.ViewModels.Schedules;

public abstract class ScheduleBaseViewModel : EditableViewModelBase, INavigationAware
{
    private readonly IAppointmentService _appointmentService;
    private readonly IDoctorService _doctorService;
    private readonly IMapper _mapper;
    private readonly INavigationService _navigationService;
    private readonly IPatientService _patientService;
    private IEnumerable<DoctorDTO> _allDoctors = new List<DoctorDTO>();
    private IEnumerable<Patient> _allPatients = new List<Patient>();
    private Appointment _appointment = null!;
    private ObservableCollection<TimeOnly> _availableHours = new();
    private ObservableCollection<DateTime> _blackOutDates = new();
    private ObservableCollection<Doctor> _doctors = new();
    private ObservableCollection<Patient> _patients = new();
    private DateTime? _selectedDate;
    private Doctor? _selectedDoctor = new();
    private Patient? _selectedPatient = new();
    private Speciality? _selectedSpeciality = Speciality.GeneralPractice;
    private TimeOnly _selectedStartTime;
    protected List<TimeOnly> DoctorAvailableHours = new();

    protected ScheduleBaseViewModel(IModalService modalService,
        INavigationService navigationService,
        IAppointmentService appointmentService,
        IMapper mapper,
        IDoctorService doctorService,
        IPatientService patientService) : base(modalService)
    {
        _navigationService = navigationService;
        _appointmentService = appointmentService;
        _mapper = mapper;
        _doctorService = doctorService;
        _patientService = patientService;
    }
    public DateTime EndingDate { get; set; } = DateHelper.GenerateDatesUntilNextMonth();

    public virtual Appointment Appointment
    {
        get => _appointment;
        set
        {
            _appointment = value;
            NotifyPropertyChanged();
        }
    }

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

    public ObservableCollection<DateTime> BlackoutDates
    {
        get => _blackOutDates;
        set
        {
            _blackOutDates = value;
            NotifyPropertyChanged();
        }
    }

    public ObservableCollection<TimeOnly> AvailableHours
    {
        get => _availableHours;
        protected set
        {
            _availableHours = value;
            NotifyPropertyChanged();
        }
    }

    [Required(ErrorMessage = "The Patient field is required")]
    public Patient? SelectedPatient
    {
        get => _selectedPatient;
        set
        {
            _selectedPatient = value;
            if (value != null)
            {
                Appointment.Patient = value;
                Appointment.PatientId = value.Id;
            }
            NotifyPropertyChanged();
        }
    }

    [Required(ErrorMessage = "The Doctor field is required")]
    public Doctor? SelectedDoctor
    {
        get => _selectedDoctor;
        set
        {
            _selectedDoctor = value;
            if (value != null)
            {
                Appointment.Doctor = value;
                Appointment.DoctorId = value.Id;
            }
            NotifyPropertyChanged();
            FilterTimes();
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
        get => _selectedDate;
        set
        {
            _selectedDate = value;
            if (value != null) Appointment.Start = value;
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
            if (Appointment.Start != null)
            {
                Appointment.Start = DateHelper.CombineDateAndTime(Appointment.Start.Value, value);
                Appointment.End = Appointment.Start.Value.AddMinutes(Appointment.Duration);
            }
            NotifyPropertyChanged();
        }
    }

    public void OnNavigatedTo() { }

    public void OnNavigatedFrom()
    {
        Dispose();
    }

    public override void Dispose()
    {
        Appointment.PropertyChanged -= OnPropertyChanged;
        Appointment.ErrorsChanged -= RaiseCanChange;
    }

    protected override bool CanSave()
    {
        return !Appointment.HasErrors
            && SelectedPatient != null
            && SelectedSpeciality != null
            && SelectedDate != null
            && SelectedDoctor != null;
    }

    protected void ResetAvailableHours()
    {
        var timeRange = new TimeRange(Appointment.StartingHour, Appointment.EndingHour).Times;
        AvailableHours = new ObservableCollection<TimeOnly>(timeRange);
    }

    protected override void OnGoBack()
    {
        _navigationService.Navigate(typeof(ScheduleList));
    }

    protected override void SubscribeChanges()
    {
        Appointment.PropertyChanged += OnPropertyChanged;
        Appointment.ErrorsChanged += RaiseCanChange;
    }

    protected async virtual void FilterTimes()
    {
        if (SelectedPatient == null || SelectedDate == null || SelectedDoctor == null || SelectedDoctor.Id == 0)
            return;
        var selectedDoctor = _mapper.Map<DoctorDTO>(SelectedDoctor);
        var selectedDate = SelectedDate.Value;
        DoctorAvailableHours = await _appointmentService.GetAvailableHoursByDoctor(selectedDoctor, selectedDate);
        AvailableHours = new ObservableCollection<TimeOnly>(DoctorAvailableHours);
    }

    protected async Task LoadCollectionsData()
    {
        _allDoctors = await _doctorService.GetDoctors();
        _allPatients = _mapper.Map<List<Patient>>(await _patientService.GetPatients());
        Doctors = new ObservableCollection<Doctor>(_mapper.Map<IEnumerable<Doctor>>(_allDoctors));
        Patients = new ObservableCollection<Patient>(_allPatients);
    }

    protected abstract void SetDefaultValues();

    protected override void OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (sender != null) IsTouched = !sender.CheckAnyStringNullOrEmpty(false);
        SaveCommand.RaiseCanExecuteChanged();
    }

    protected async virtual void FilterDoctors()
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

    private void ClearDates()
    {
        SelectedDate = null;
        BlackoutDates.Clear();
    }
}
