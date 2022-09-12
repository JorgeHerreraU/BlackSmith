using BlackSmith.Business.Components;
using BlackSmith.Business.Interfaces;
using BlackSmith.Core.ExtensionMethods;
using BlackSmith.Domain.Models;

namespace BlackSmith.Business.ComplexValidators;

public class DoctorMissingDayValidation : IUpdateComplexValidation<Doctor>
{
    private readonly AppointmentsDoctorsBL _appointmentsDoctorsBL;

    public DoctorMissingDayValidation(AppointmentsDoctorsBL appointmentsDoctorsBL)
    {
        _appointmentsDoctorsBL = appointmentsDoctorsBL;
    }
    public async Task ValidateAsync(Doctor doctor)
    {
        var appointments = (await _appointmentsDoctorsBL.GetUpcomingAppointmentsByDoctor(doctor)).ToList();
        var appointmentsDayOfWeek = appointments.Select(x => x.Start.DayOfWeek).Distinct().ToList();
        var incomingDayOfWeek = doctor.WorkingDays.Select(x => x.Day).ToList();
        if (appointmentsDayOfWeek.HasNotAll(incomingDayOfWeek))
            throw new ArgumentException("Doctor has one or more appointments, please review the agenda");
    }
}
