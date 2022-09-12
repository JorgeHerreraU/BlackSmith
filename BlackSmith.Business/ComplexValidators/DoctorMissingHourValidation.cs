using BlackSmith.Business.Components;
using BlackSmith.Business.Interfaces;
using BlackSmith.Domain.Models;

namespace BlackSmith.Business.ComplexValidators;

public class DoctorMissingHourValidation : IUpdateComplexValidation<Doctor>
{
    private readonly AppointmentsDoctorsBL _appointmentsDoctorsBL;

    public DoctorMissingHourValidation(AppointmentsDoctorsBL appointmentsDoctorsBL)
    {
        _appointmentsDoctorsBL = appointmentsDoctorsBL;
    }
    public async Task ValidateAsync(Doctor doctor)
    {
        var appointments = (await _appointmentsDoctorsBL.GetUpcomingAppointmentsByDoctor(doctor)).ToList();
        var appointmentDates = appointments.Select(x => x.Start).ToList();
        var incomingWorkingDays = doctor.WorkingDays;
        var matchingDays = appointmentDates.Where(d => incomingWorkingDays.Any(w => w.Day == d.DayOfWeek));
        var hasMissingWorkingHour = (from scheduled in matchingDays
            let workingRange = _appointmentsDoctorsBL.GetWorkingTimes(incomingWorkingDays, scheduled.DayOfWeek)
            where scheduled.Hour <= workingRange.Start.Hour || scheduled.Hour >= workingRange.End.Hour
            select scheduled).Any();
        if (hasMissingWorkingHour)
            throw new ArgumentException("Doctor has one or more appointments, please review the agenda");
    }
}
