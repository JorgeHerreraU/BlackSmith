using AutoMapper;
using BlackSmith.Presentation.Enums;
using BlackSmith.Presentation.Extensions;
using BlackSmith.Presentation.Helpers;
using BlackSmith.Presentation.Interfaces;
using BlackSmith.Presentation.Models;
using BlackSmith.Presentation.Views.Pages;
using BlackSmith.Service.DTOs;
using BlackSmith.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Wpf.Ui.Mvvm.Contracts;

namespace BlackSmith.Presentation.ViewModels;

public class ScheduleCreateViewModel : EditableViewModelBase
{
    private readonly IAppointmentService _appointmentService;
    private readonly IDoctorService _doctorService;
    private readonly IMapper _mapper;
    private readonly INavigationService _navigationService;
    private readonly IPatientService _patientService;

    private IEnumerable<Doctor> _allDoctors = new List<Doctor>();
    private ObservableCollection<DateTime> _blackOutDates = new();
    private ObservableCollection<Doctor> _doctors = null!;
    private ObservableCollection<TimeOnly> _availableHours = new();

    private Appointment _appointment = null!;

    private Speciality _selectedSpeciality = Speciality.GeneralPractice;
    private DateTime? _selectedDate = new();
    private TimeOnly _selectedStartTime = new();
    private Doctor _selectedDoctor = new();
    private Patient _selectedPatient = new();


    public ScheduleCreateViewModel(IAppointmentService appointmentService,
        IModalService modalService,
        IMapper mapper,
        INavigationService navigationService,
        IDoctorService doctorService,
        IPatientService patientService) : base(modalService)
    {
        _appointmentService = appointmentService;
        _mapper = mapper;
        _navigationService = navigationService;
        _doctorService = doctorService;
        _patientService = patientService;
    }

    public Appointment Appointment
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

    public DateTime EndingDate { get; set; } = new
        DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(1).AddDays(7);

    public ObservableCollection<Doctor> Doctors
    {
        get => _doctors;
        private set
        {
            _doctors = value;
            NotifyPropertyChanged();
        }
    }

    public IEnumerable<Patient> Patients { get; set; } = new List<Patient>();

    [Required]
    public Speciality SelectedSpeciality
    {
        get => _selectedSpeciality;
        set
        {
            _selectedSpeciality = value;
            NotifyPropertyChanged();
            FilterDatesByDoctorSpeciality();
        }
    }

    [Required(ErrorMessage = "The Time field is required")]
    public DateTime? SelectedDate
    {
        get => _selectedDate;
        set
        {
            _selectedDate = value;
            Appointment.Start = value;
            NotifyPropertyChanged();
            FilterDoctors();
            FilterTimesByDoctor();
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
                Appointment.Start = SelectedDate.Value.Add(value.ToTimeSpan());
                Appointment.End = Appointment.Start.Value.AddMinutes(Appointment.Duration);
            }

            NotifyPropertyChanged();
        }
    }

    [Required(ErrorMessage = "The Doctor field is required")]
    public Doctor SelectedDoctor
    {
        get => _selectedDoctor;
        set
        {
            _selectedDoctor = value;
            Appointment.Doctor = value;
            NotifyPropertyChanged();
            FilterTimesByDoctor();
        }
    }

    private void FilterTimesByDoctor()
    {
        if (SelectedDate is null || SelectedDoctor is null || SelectedDoctor?.Id == 0)
            return;

        var workingDay = _allDoctors.First(x => x.Id == SelectedDoctor?.Id)
            .WorkingDays
            .First(x => x.Day == SelectedDate.Value.DayOfWeek);

        AvailableHours = new ObservableCollection<TimeOnly>(
            TimeHelper.GetTimeRange(workingDay.StartTime, workingDay.EndTime));

        SelectedStartTime = AvailableHours.First();
    }

    [Required(ErrorMessage = "The Patient field is required")]
    public Patient SelectedPatient
    {
        get => _selectedPatient;
        set
        {
            _selectedPatient = value;
            Appointment.Patient = value;
            NotifyPropertyChanged();
        }
    }

    private async void FilterDatesByDoctorSpeciality()
    {
        ClearDates();

        var availableDays = await _appointmentService.GetAvailableDaysByDoctorsSpeciality(
            SelectedSpeciality.ToSpecialityDTO(),
            DateTime.Now,
            EndingDate);


        foreach (var day in DateHelper.GetDateRange(DateTime.Now, EndingDate))
        {
            if (!availableDays.Contains(day))
                BlackoutDates.Add(day);
        }
    }

    private void ClearDates()
    {
        SelectedDate = null;
        BlackoutDates.Clear();
    }

    private IEnumerable<Doctor> GetDoctorsBySpeciality()
    {
        return _allDoctors.Where(d => d.Speciality == SelectedSpeciality);
    }

    public ObservableCollection<TimeOnly> AvailableHours
    {
        get => _availableHours;
        set
        {
            _availableHours = value;
            NotifyPropertyChanged();
        }
    }

    private void ResetAvailableHours()
    {
        AvailableHours = new ObservableCollection<TimeOnly>(
            TimeHelper.GetTimeRange(new TimeOnly(9, 0, 0), new TimeOnly(18, 0, 0)));
    }

    private void FilterDoctors()
    {
        if (SelectedDate is null)
        {
            Doctors = new ObservableCollection<Doctor>();
            return;
        }

        var doctors = GetDoctorsBySpeciality()
            .Where(d => d.WorkingDays.Select(x => x.Day)
            .Contains(SelectedDate.Value.DayOfWeek))
            .ToAsyncEnumerable()
            .WhereAwait(async d => !await GetDoctorFullyBooked(d, SelectedDate.Value))
            .ToEnumerable();

        Doctors = new ObservableCollection<Doctor>(doctors);
    }

    protected override void OnGoBack()
    {
        _navigationService.Navigate(typeof(ScheduleList));
    }

    protected override bool CanSave()
    {
        return !Appointment.HasErrors;
    }

    protected override void OnSave()
    {
        Appointment.ToString();
    }

    public override async void Set()
    {
        Appointment = new();

        await SetCollectionsData();

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

    private async Task SetCollectionsData()
    {
        _allDoctors = _mapper.Map<IEnumerable<Doctor>>(await _doctorService.GetDoctors());
        Patients = _mapper.Map<IEnumerable<Patient>>(await _patientService.GetPatients());
    }

    private async Task<bool> GetDoctorFullyBooked(Doctor doctor, DateTime date)
    {
        return await _appointmentService.GetDoctorFullyBooked(_mapper.Map<DoctorDTO>(doctor),
            date);
    }

    public override void SubscribeChanges()
    {
        Appointment.PropertyChanged += OnPropertyChanged;
        Appointment.ErrorsChanged += RaiseCanChange;
    }
}