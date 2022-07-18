using BlackSmith.Domain.Models;

namespace BlackSmith.Data;

public static class SeedData
{
    public static void Initialize()
    {
        using var context = new AppDbContextFactory().CreateDbContext();

        if (context.Patients.Any()) return;

        var address = new Address
        {
            Street = "Boulevard",
            Number = 100,
            City = "Adelaide",
            State = "Northwind",
            Country = "United States",
            ZipCode = 392982
        };

        var workingDays = new List<WorkingDay>
        {
            new() { StartTime = new TimeOnly(10, 00), EndTime = new TimeOnly(18, 00), Day = DayOfWeek.Monday },
            new() { StartTime = new TimeOnly(10, 00), EndTime = new TimeOnly(18, 00), Day = DayOfWeek.Tuesday },
            new() { StartTime = new TimeOnly(10, 00), EndTime = new TimeOnly(18, 00), Day = DayOfWeek.Wednesday }
        };

        var doctor = new Doctor
        {
            FirstName = "Ron",
            LastName = "Howard",
            Age = 42,
            Email = "ronhoward@email.com",
            Phone = "1-808-874-8432",
            Speciality = Speciality.Cardiology,
            WorkingDays = workingDays,
            Address = address
        };

        context.Doctors.Add(doctor);
        context.SaveChanges();

        var patient = new Patient
        {
            FirstName = "Lucas",
            LastName = "San Toro",
            Email = "lucassantoro@email.com",
            Identification = "F38492282",
            Phone = "1-808-874-8432",
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