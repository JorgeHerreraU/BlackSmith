using BlackSmith.Business.Components;
using BlackSmith.Business.Interfaces;
using BlackSmith.Domain.Models;

namespace BlackSmith.Business.ComplexValidators;

public class AppointmentFullValidation : ICreateComplexValidation<Appointment>,
    IUpdateComplexValidation<Appointment>
{
    private readonly AppointmentsDoctorsBL _appointmentsDoctorsBL;

    public AppointmentFullValidation(AppointmentsDoctorsBL appointmentsDoctorsBL)
    {
        _appointmentsDoctorsBL = appointmentsDoctorsBL;
    }
    public async Task ValidateAsync(Appointment appointment)
    {
        var isDoctorFullyBooked = await _appointmentsDoctorsBL.GetDoctorIsFullyBookedOnSpecificDate(
            appointment.Doctor,
            appointment.Start
        );
        if (isDoctorFullyBooked) throw new ArgumentException("Doctor is fully booked on this day");
    }
}
