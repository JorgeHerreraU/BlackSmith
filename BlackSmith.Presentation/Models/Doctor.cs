namespace BlackSmith.Presentation.Models;

public class Doctor
{
    public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";
    public string Email { get; set; } = "";
    public string Phone { get; set; } = "";
    public Speciality Speciality { get; set; } = Speciality.GeneralPractice;
    public Address Address { get; set; } = new();
}