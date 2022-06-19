using System.ComponentModel.DataAnnotations;

namespace BlackSmith.Service.DTOs;

public enum Speciality
{
    Allergology,
    Anesthesiology,
    Cardiology,
    Dermatology,
    Endocrinology,
    Gastroenterology,
    GeneralPractice,
    Hematology,
    InfectiousDiseases,
    Nephrology,
    Neurology,
    Oncology,
    Ophthalmology,
    Orthopedics,
    Osteopathy,
    Pathology,
    Pediatrics,
    PhysicalMedicine,
    Psychiatry,
    Radiology,
    Rheumatology,
    Surgery,
    Urology
}

public class DoctorDTO
{
    [Required] public int Id { get; set; }

    [Required] public string FirstName { get; set; } = "";

    [Required] public string LastName { get; set; } = "";

    [Required] public string Email { get; set; } = "";

    [Required] public string Phone { get; set; } = "";
    [Required] public int Age { get; set; }

    [Required] public Speciality Speciality { get; set; } = Speciality.GeneralPractice;

    [Required] public AddressDTO Address { get; set; } = new();
    [Required] public ICollection<WorkingDayDTO> WorkingDays { get; set; } = new List<WorkingDayDTO>();
}