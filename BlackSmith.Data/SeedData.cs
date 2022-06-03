using BlackSmith.Domain.Models;

namespace BlackSmith.Data;

public static class SeedData
{
    public static void Initialize()
    {
        using var context = new AppDbContextFactory().CreateDbContext();

        if (context.Appointments.Any()) return;

        var address = new Address
        {
            Street = "Boulevard",
            Number = 100,
            City = "Adelaide",
            State = "Northwind"
        };

        var workingDays = new List<WorkingDay>
        {
            new() { StartTime = DateTime.Now, EndTime = DateTime.Now, Day = DayOfWeek.Monday }
        };

        var doctor = new Doctor
        {
            FirstName = "Ron",
            LastName = "Howard",
            Email = "ronhoward@email.com",
            Phone = "1-808-87484329",
            Speciality = Speciality.Cardiology,
            WorkingDays = workingDays
        };

        context.Doctors.Add(doctor);
        context.SaveChanges();

        var patient = new Patient
        {
            FirstName = "Lucas",
            LastName = "San Toro",
            Email = "lucassantoro@email.com",
            PhoneNumber = "1-807-84832991",
            Age = 32,
            Address = address
        };

        context.Patients.Add(patient);
        context.SaveChanges();

        var appointment = new Appointment
        {
            Title = "General Medicine Request",
            Description = "Patient feels bad",
            Patient = patient,
            Doctor = doctor,
            Start = DateTime.Now,
            End = DateTime.Now
        };

        context.Appointments.Add(appointment);
        context.SaveChanges();
    }
}