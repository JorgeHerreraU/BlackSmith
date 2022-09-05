using AutoMapper;
using BlackSmith.Core.Helpers;
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
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Wpf.Ui.Common.Interfaces;
using Wpf.Ui.Mvvm.Contracts;

namespace BlackSmith.Presentation.ViewModels.Schedules;

public class ScheduleEditViewModel : EditableViewModelBase, INavigationAware
{
    private readonly IAppointmentService _appointmentService;
    private readonly IDoctorService _doctorService;
    private readonly IMapper _mapper;
    private readonly INavigationService _navigationService;
    private readonly IPatientService _patientService;
    private List<Doctor> _allDoctors = new();
    private Appointment _appointment = null!;
    private ObservableCollection<TimeOnly> _availableHours = new();
    private ObservableCollection<DateTime> _blackOutDates = new();
    private ObservableCollection<Doctor> _doctors = new();
    private DateTime? _selectedDate;
    private Speciality? _selectedSpeciality = Speciality.GeneralPractice;
    private TimeOnly _selectedStartTime;

    public ScheduleEditViewModel(
        IModalService modalService,
        IPatientService patientService,
        IMapper mapper,
        INavigationService navigationService,
        IDoctorService doctorService,
        IAppointmentService appointmentService
    ) : base(modalService)
    {
        _patientService = patientService;
        _mapper = mapper;
        _navigationService = navigationService;
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
            Appointment.Patient = value!;
            if (value != null)
                Appointment.PatientId = value.Id;
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
            FilterDatesByDoctorSpeciality();
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
            FilterTimesByDoctor();
        }
    }

    public List<Patient> Patients
    {
        get;
        private set;
    } = new();

    [Required(ErrorMessage = "The Doctor field is required")]
    public Doctor? SelectedDoctor
    {
        get => Appointment.Doctor;
        set
        {
            Appointment.Doctor = value!;
            if (value != null)
                Appointment.DoctorId = value.Id;
            NotifyPropertyChanged();
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
                Appointment.Start = SelectedDate.Value.Date + _selectedStartTime.ToTimeSpan();
                Appointment.End = Appointment.Start.Value.AddMinutes(Appointment.Duration);
            }
            NotifyPropertyChanged();
        }
    }

    public ObservableCollection<TimeOnly> AvailableHours
    {
        get => _availableHours;
        private set
        {
            _availableHours = value;
            NotifyPropertyChanged();
        }
    }

    public void OnNavigatedTo() { }

    public void OnNavigatedFrom()
    {
        Dispose();
    }

    public override async void Initialize()
    {
        await LoadCollectionsData();
        ResetAvailableHours();
        SubscribeChanges();
        SetAppointmentValues();
    }

    private void SetAppointmentValues()
    {
        IsTouched = true;
        SelectedSpeciality = Doctors.First(d => d.Id == Appointment.DoctorId).Speciality;
        SelectedPatient = Patients.FirstOrDefault(p => p.Id == Appointment.PatientId);
        SelectedDoctor = Doctors.FirstOrDefault(d => d.Id == Appointment.DoctorId);
        SelectedDate = Appointment.Start;
    }

    public override void SubscribeChanges()
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

    private void FilterDoctors()
    {
        if (SelectedDate is null)
        {
            Doctors = new ObservableCollection<Doctor>();
            return;
        }
        var doctors = GetAllDoctorsBySpeciality()
            .Where(d => d.WorkingDays.Select(x => x.Day).Contains(SelectedDate.Value.DayOfWeek))
            .ToAsyncEnumerable()
            .WhereAwait(async d => !await GetDoctorFullyBooked(d, SelectedDate.Value))
            .ToEnumerable();
        Doctors = new ObservableCollection<Doctor>(doctors);
        SelectedDoctor = Doctors.FirstOrDefault(d => d.Id == Appointment.DoctorId);
    }

    private async void FilterDatesByDoctorSpeciality()
    {
        if (SelectedSpeciality == null)
            return;
        ClearDates();
        var availableDays = (await _appointmentService.GetAvailableDaysByDoctorsSpeciality(
            SelectedSpeciality.Value.ToSpecialityDTO(),
            DateTime.Now,
            EndingDate
        )).ToList();
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

    private void ResetAvailableHours()
    {
        AvailableHours = new ObservableCollection<TimeOnly>(
            TimeHelper.GetTimeRange(new TimeOnly(9, 0, 0), new TimeOnly(18, 0, 0))
        );
    }

    private IEnumerable<Doctor> GetAllDoctorsBySpeciality()
    {
        return _allDoctors.Where(d => d.Speciality == SelectedSpeciality);
    }

    private async Task<bool> GetDoctorFullyBooked(Doctor doctor, DateTime date)
    {
        return await _appointmentService.GetDoctorFullyBooked(_mapper.Map<DoctorDTO>(doctor), date);
    }

    private async Task LoadCollectionsData()
    {
        _allDoctors = _mapper.Map<List<Doctor>>(await _doctorService.GetDoctors());
        Doctors = new ObservableCollection<Doctor>(_mapper.Map<List<Doctor>>(_allDoctors));
        Patients = _mapper.Map<List<Patient>>(await _patientService.GetPatients());
    }

    private void FilterTimesByDoctor()
    {
        if (SelectedDate is null || SelectedDoctor is null || SelectedDoctor?.Id == 0)
            return;
        var workingDay = GetSelectedDoctorWorkingDay();
        AvailableHours = new ObservableCollection<TimeOnly>(
            TimeHelper.GetTimeRange(workingDay.StartTime, workingDay.EndTime)
        );
        if (Appointment.Start != null)
            SelectedStartTime = TimeOnly.FromDateTime(Appointment.Start.Value);
    }

    private WorkingDay GetSelectedDoctorWorkingDay()
    {
        return Doctors
            .First(x => x.Id == SelectedDoctor?.Id)
            .WorkingDays.First(x => x.Day == SelectedDate!.Value.DayOfWeek);
    }

    public override void Dispose()
    {
        Appointment.PropertyChanged -= OnPropertyChanged;
        Appointment.ErrorsChanged -= RaiseCanChange;
    }
}
