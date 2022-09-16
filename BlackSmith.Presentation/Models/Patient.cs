using System.ComponentModel.DataAnnotations;

namespace BlackSmith.Presentation.Models;

public class Patient : ValidatableBase
{
    private Address _address = new();
    private int _age;
    private string _email = "";
    private string _firstName = "";
    private int _id;
    private string _identification = "";
    private string _lastName = "";
    private string _phone = "";

    public int Id
    {
        get => _id;
        set
        {
            _id = value;
            NotifyPropertyChanged();
        }
    }

    [Required(AllowEmptyStrings = false)]
    [MinLength(3)]
    [MaxLength(200)]
    public string FirstName
    {
        get => _firstName;
        set
        {
            _firstName = value;
            NotifyPropertyChanged();
        }
    }

    [MinLength(3)]
    [MaxLength(200)]
    [Required(AllowEmptyStrings = false)]
    public string LastName
    {
        get => _lastName;
        set
        {
            _lastName = value;
            NotifyPropertyChanged();
        }
    }

    [Required(AllowEmptyStrings = false)]
    [EmailAddress]
    public string Email
    {
        get => _email;
        set
        {
            _email = value;
            NotifyPropertyChanged();
        }
    }

    [Required(AllowEmptyStrings = false)]
    [Phone]
    public string Phone
    {
        get => _phone;
        set
        {
            _phone = value;
            NotifyPropertyChanged();
        }
    }

    [Required]
    [Range(1, 99)]
    public int Age
    {
        get => _age;
        set
        {
            _age = value;
            NotifyPropertyChanged();
        }
    }

    [Required]
    public Address Address
    {
        get => _address;
        set
        {
            _address = value;
            NotifyPropertyChanged();
        }
    }

    [Required(AllowEmptyStrings = false)]
    public string Identification
    {
        get => _identification;
        set
        {
            _identification = value;
            NotifyPropertyChanged();
        }
    }

    public string FullName
    {
        get => $"{FirstName} {LastName}";
    }

    public override string ToString()
    {
        return $"Patient: {FullName}, \nIdentification: {Identification}";
    }
}
