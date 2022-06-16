namespace BlackSmith.Domain.Models;

public class Doctor : BaseEntity
{
    public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";
    public string Email { get; set; } = "";
    public string Phone { get; set; } = "";
    public Speciality Speciality { get; set; } = Speciality.GeneralPractice;
    public Address Address { get; set; } = new();
    public ICollection<WorkingDay> WorkingDays { get; set; } = new HashSet<WorkingDay>();
    public ICollection<Appointment> Appointments { get; set; } = new HashSet<Appointment>();
}