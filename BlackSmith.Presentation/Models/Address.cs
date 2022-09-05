using System.ComponentModel.DataAnnotations;

namespace BlackSmith.Presentation.Models;

public class Address : ValidatableBase
{
    private string _city = "";
    private string _country = "";
    private int _id;
    private int _number;
    private string _state = "";
    private string _street = "";
    private int _zipcode;

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
    [MaxLength(250)]
    public string Street
    {
        get => _street;
        set
        {
            _street = value;
            NotifyPropertyChanged();
        }
    }

    [Required(AllowEmptyStrings = false)]
    [Range(0, 99999)]
    public int Number
    {
        get => _number;
        set
        {
            _number = value;
            NotifyPropertyChanged();
        }
    }

    [Required(AllowEmptyStrings = false)]
    [MaxLength(250)]
    public string City
    {
        get => _city;
        set
        {
            _city = value;
            NotifyPropertyChanged();
        }
    }

    [Required(AllowEmptyStrings = false)]
    [MaxLength(250)]
    public string State
    {
        get => _state;
        set
        {
            _state = value;
            NotifyPropertyChanged();
        }
    }

    [Required]
    public int ZipCode
    {
        get => _zipcode;
        set
        {
            _zipcode = value;
            NotifyPropertyChanged();
        }
    }

    [Required(AllowEmptyStrings = false)]
    [MaxLength(250)]
    public string Country
    {
        get => _country;
        set
        {
            _country = value;
            NotifyPropertyChanged();
        }
    }

    public string FullAddress
    {
        get => $"{Number} {Street}, {City}, {State}";
    }
}
