namespace BlackSmith.Domain.Models;

public class Address : BaseEntity
{
    public string Street { get; set; } = "";
    public int Number { get; set; }
    public string City { get; set; } = "";
    public string State { get; set; } = "";
    public int ZipCode { get; set; }
    public string Country { get; set; } = "";
}
