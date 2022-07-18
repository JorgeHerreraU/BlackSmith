using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using BlackSmith.Presentation.Enums;

namespace BlackSmith.Presentation.Models;

public class Doctor : ValidatableBase
{
    private Address _address = new();
    private int _age;
    private string _email = "";
    private string _firstName = "";
    private int _id;
    private string _lastName = "";
    private string _phone = "";
    private Speciality _speciality = Speciality.GeneralPractice;
    private ICollection<WorkingDay> _workingDays = new List<WorkingDay>();

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

    [Required(AllowEmptyStrings = false)]
    [MinLength(3)]
    [MaxLength(200)]
    public string LastName
    {
        get => _lastName;
        set
        {
            _lastName = value;
            NotifyPropertyChanged();
        }
    }

    [Required]
    [Range(18, 99)]
    public int Age
    {
        get => _age;
        set
        {
            _age = value;
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
    public Address Address

    {
        get => _address;
        set
        {
            _address = value;
            NotifyPropertyChanged();
        }
    }

    [Required]
    public ICollection<WorkingDay> WorkingDays
    {
        get => _workingDays;
        set
        {
            _workingDays = value;
            NotifyPropertyChanged();
        }
    }

    [Required]
    public Speciality Speciality
    {
        get => _speciality;
        set
        {
            _speciality = value;
            NotifyPropertyChanged();
        }
    }

    public string FullName => $"{FirstName} {LastName}";

    public string DaysWorking => string.Join(", ", WorkingDays.Select(x => x.Day.ToString()).ToList());
}