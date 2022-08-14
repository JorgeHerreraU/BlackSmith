using AutoMapper;
using BlackSmith.Presentation.Commands;
using BlackSmith.Presentation.Enums;
using BlackSmith.Presentation.Helpers;
using BlackSmith.Presentation.Models;
using BlackSmith.Presentation.Views.Pages;
using BlackSmith.Service.Interfaces;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Wpf.Ui.Mvvm.Contracts;

namespace BlackSmith.Presentation.ViewModels;

public class ScheduleCreateViewModel : BindableBase
{
    private readonly IAppointmentService _appointmentService;
    private readonly IDoctorService _doctorService;
    private readonly IMapper _mapper;
    private readonly INavigationService _navigationService;
    private readonly IPatientService _patientService;

    private IEnumerable<Doctor> _allDoctors = new List<Doctor>();
    private IEnumerable<Appointment> _allAppointments = new List<Appointment>();
    private ObservableCollection<DateTime> _blackOutDates = new();
    private ObservableCollection<Doctor> _doctors = null!;

    private Speciality _selectedSpeciality = Speciality.GeneralPractice;

    private TimeOnly _selectedStartTime = new();
    private Doctor _selectedDoctor = new();
    private Patient _selectedPatient = new();


    public ScheduleCreateViewModel(IAppointmentService appointmentService,
        IMapper mapper,
        INavigationService navigationService,
        IDoctorService doctorService,
        IPatientService patientService)
    {
        _appointmentService = appointmentService;
        _mapper = mapper;
        _navigationService = navigationService;
        _doctorService = doctorService;
        _patientService = patientService;

        GoBack = new RelayCommand(OnGoBack);
    }

    public RelayCommand GoBack { get; }

    public ObservableCollection<DateTime> BlackoutDates
    {
        get => _blackOutDates;
        set
        {
            _blackOutDates = value;
            RaisePropertyChanged();
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
            RaisePropertyChanged();
        }
    }

    public IEnumerable<Patient> Patients { get; set; } = new List<Patient>();

    public Speciality SelectedSpeciality
    {
        get => _selectedSpeciality;
        set
        {
            _selectedSpeciality = value;
            RaisePropertyChanged();
            FilterDatesByDoctorSpeciality();
            FilterDoctors();
        }
    }

    public TimeOnly SelectedStartTime
    {
        get => _selectedStartTime;
        set
        {
            _selectedStartTime = value;
            RaisePropertyChanged();
        }
    }

    public Doctor SelectedDoctor
    {
        get => _selectedDoctor;
        set
        {
            _selectedDoctor = value;
            RaisePropertyChanged();
        }
    }

    public Patient SelectedPatient
    {
        get => _selectedPatient;
        set
        {
            _selectedPatient = value;
            RaisePropertyChanged();
        }
    }

    private void FilterDatesByDoctorSpeciality()
    {
        BlackoutDates.Clear();

        var days = DateHelper.GetDateRange(DateTime.Now, EndingDate);
        var doctorsWorkingDays = GetDoctorsBySpeciality().SelectMany(d => d.WorkingDays);


        foreach (var day in days)
        {
            var iSNotAWorkingDay = !doctorsWorkingDays.Select(x => x.Day).Distinct().Contains(day.DayOfWeek);

            if (iSNotAWorkingDay)
            {
                BlackoutDates.Add(day);
            }
            else
            {
                var workStartTime = doctorsWorkingDays.Where(x => x.Day == day.DayOfWeek).Select(x => x.StartTime).Min();
                var workFinishTime = doctorsWorkingDays.Where(x => x.Day == day.DayOfWeek).Select(x => x.EndTime).Max();
            }
        }
    }

    private IEnumerable<Doctor> GetDoctorsBySpeciality()
    {
        return _allDoctors.Where(d => d.Speciality == SelectedSpeciality);
    }

    public ObservableCollection<TimeOnly> AvailableHours { get; } = new()
    {
        new TimeOnly(9, 0, 0),
        new TimeOnly(10, 0, 0),
        new TimeOnly(11, 0, 0),
        new TimeOnly(12, 0, 0),
        new TimeOnly(13, 0, 0),
        new TimeOnly(14, 0, 0),
        new TimeOnly(15, 0, 0),
        new TimeOnly(16, 0, 0),
        new TimeOnly(17, 0, 0),
        new TimeOnly(18, 0, 0)
    };

    private void FilterDoctors()
    {
        var doctors = _allDoctors.Where(d => d.Speciality == SelectedSpeciality);
        Doctors = new ObservableCollection<Doctor>(doctors);
    }

    public async void SetNewSchedule()
    {
        var doctors = await _doctorService.GetDoctors();
        _allDoctors = _mapper.Map<IEnumerable<Doctor>>(doctors);

        var appointments = await _appointmentService.GetAppointments();
        _allAppointments = _mapper.Map<IEnumerable<Appointment>>(appointments);

        Patients = _mapper.Map<List<Patient>>(await _patientService.GetPatients());
    }
    private void OnGoBack()
    {
        _navigationService.Navigate(typeof(ScheduleList));
    }

}