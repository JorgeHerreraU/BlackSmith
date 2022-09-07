using BlackSmith.Presentation.Enums;
using BlackSmith.Presentation.Interfaces;
using BlackSmith.Presentation.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BlackSmith.Presentation.Filters;

public class DoctorFilter : IFilter<Doctor>
{
    public IEnumerable<Doctor> Filter(IEnumerable<Doctor> items,
        ISpecification<Doctor> specification)
    {
        return items.Where(specification.IsSatisfied);
    }
}

public class DoctorSpecialitySpecification : ISpecification<Doctor>
{
    private readonly Speciality _speciality;
    public DoctorSpecialitySpecification(Speciality speciality)
    {
        _speciality = speciality;
    }
    public bool IsSatisfied(Doctor doctor)
    {
        return _speciality == doctor.Speciality;
    }
}

public class DoctorWorkingDaysSpecification : ISpecification<Doctor>
{
    private readonly DayOfWeek _dayOfWeek;
    public DoctorWorkingDaysSpecification(DayOfWeek dayOfWeek)
    {
        _dayOfWeek = dayOfWeek;
    }

    public bool IsSatisfied(Doctor doctor)
    {
        return doctor.WorkingDays.Select(w => w.Day).Contains(_dayOfWeek);
    }
}

public class DoctorFullyBookedSpecification : ISpecification<Doctor>
{
    private readonly IEnumerable<DateTime> _availableDays;
    private readonly DateTime _dateTime;

    public DoctorFullyBookedSpecification(
        DateTime dateTime,
        IEnumerable<DateTime> availableDays)
    {
        _availableDays = availableDays;
        _dateTime = dateTime;
    }
    public bool IsSatisfied(Doctor doctor)
    {
        return _availableDays.Contains(_dateTime.Date);
    }
}
