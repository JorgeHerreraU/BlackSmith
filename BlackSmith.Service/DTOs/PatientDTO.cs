using System.ComponentModel.DataAnnotations;

namespace BlackSmith.Service.DTOs;

public class PatientDTO
{
    [Required] public int Id { get; set; }
    [Required] public string FirstName { get; set; } = "";
    [Required] public string LastName { get; set; } = "";
    [Required] public string Email { get; set; } = "";
    [Required] public string Phone { get; set; } = "";
    [Required] public int Age { get; set; }
    [Required] public AddressDTO Address { get; set; } = new();
}