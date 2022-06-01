namespace BlackSmith.Presentation.Models;

public class Patient : BindableBase
{
    public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";
    public string Email { get; set; } = "";
    public string PhoneNumber { get; set; } = "";
    public Address Address { get; set; } = new();
}