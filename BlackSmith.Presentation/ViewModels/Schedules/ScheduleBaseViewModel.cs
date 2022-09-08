using AutoMapper;
using BlackSmith.Core.Helpers;
using BlackSmith.Presentation.Enums;
using BlackSmith.Presentation.Extensions;
using BlackSmith.Presentation.Filters;
using BlackSmith.Presentation.Interfaces;
using BlackSmith.Presentation.Models;
using BlackSmith.Service.DTOs;
using BlackSmith.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Wpf.Ui.Common.Interfaces;

namespace BlackSmith.Presentation.ViewModels.Schedules;

public abstract class ScheduleBaseViewModel : EditableViewModelBase, INavigationAware
{
    private readonly IAppointmentService _appointmentService;
    private readonly DoctorFilter _doctorFilter;
    private readonly IMapper _mapper;
    private ObservableCollection<TimeOnly> _availableHours = new();
    protected ScheduleBaseViewModel(IModalService modalService,
        IAppointmentService appointmentService,
        IMapper mapper,
        DoctorFilter doctorFilter) : base(modalService)
    {
        _appointmentService = appointmentService;
        _mapper = mapper;
        _doctorFilter = doctorFilter;
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

    public void OnNavigatedTo() { }

    public void OnNavigatedFrom()
    {
        Dispose();
    }

    protected async Task<List<TimeOnly>> GetDoctorAvailableHours(Doctor doctor, DateTime date)
    {
        var availableHours = await _appointmentService.GetAvailableHoursByDoctor(
            _mapper.Map<DoctorDTO>(doctor),
            date);
        return availableHours.Select(TimeOnly.FromDateTime).ToList();
    }

    protected void ResetAvailableHours()
    {
        AvailableHours = new ObservableCollection<TimeOnly>(
            TimeHelper.GetTimeRange(Appointment.StartingHour, Appointment.EndingHour).ToList());
    }

    protected async Task<List<DateTime>> GetSpecialityAvailableDays(Speciality speciality,
        DateTime start,
        DateTime end)
    {
        return (await _appointmentService.GetAvailableDaysByDoctorsSpeciality(
            speciality.ToSpecialityDTO(),
            start,
            end
        )).ToList();
    }

    protected IEnumerable<Doctor> GetFilteredDoctors(Speciality speciality,
        DateTime date,
        IEnumerable<DateTime> availableDays,
        IEnumerable<Doctor> doctors
    )
    {
        var specifications = new AndSpecification<Doctor>(new List<ISpecification<Doctor>>
        {
            new DoctorSpecialitySpecification(speciality),
            new DoctorWorkingDaysSpecification(date.DayOfWeek),
            new DoctorFullyBookedSpecification(date, availableDays)
        });
        return _doctorFilter.Filter(doctors, specifications);
    }
}
