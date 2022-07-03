namespace BlackSmith.Domain.Models;

public class Patient : BaseEntity
{
    public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";
    public string Email { get; set; } = "";
    public string Phone { get; set; } = "";
    public int Age { get; set; }
    public string Identification { get; set; } = "";
    public Address Address { get; set; } = new();
    public ICollection<Appointment> Appointments { get; set; } = new HashSet<Appointment>();
}