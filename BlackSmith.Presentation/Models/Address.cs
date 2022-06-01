namespace BlackSmith.Presentation.Models;

public class Address : BindableBase
{
    public string Street { get; set; } = "";
    public int Number { get; set; }
    public string City { get; set; } = "";
    public string State { get; set; } = "";
}