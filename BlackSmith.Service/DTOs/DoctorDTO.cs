using BlackSmith.Domain.Models;

namespace BlackSmith.Service.DTOs;

public class DoctorDTO
{
    public int Id { get; set; }
    public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";
    public string Email { get; set; } = "";
    public string Phone { get; set; } = "";
    public int Age { get; set; }
    public Speciality Speciality { get; set; } = Speciality.GeneralPractice;
    public AddressDTO Address { get; set; } = new();
    public ICollection<WorkingDayDTO> WorkingDays { get; set; } = new List<WorkingDayDTO>();
}
