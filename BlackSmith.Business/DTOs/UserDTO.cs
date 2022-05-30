using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace BlackSmith.Business.DTOs;
public class UserDTO
{
    public int Id { get; set; }

    private string _firstName = "";
    public string FirstName
    {
        get { return _firstName; }
        set
        {
            if (value.Length < 2)
            {
                throw new ArgumentException("First name must be at least 2 characters long");
            }
            _firstName = value;
        }
    }
    private string _lastName = "";
    public string LastName
    {
        get { return _lastName; }
        set
        {
            if (value.Length < 2)
            {
                throw new ArgumentException("Last name must be at least 2 characters long");
            }
            _lastName = value;
        }
    }
    private string _email = "";
    public string Email
    {
        get { return _email; }
        set
        {
            MailAddress m = new(value);
            if (m.Address != value)
            {
                throw new ArgumentException("Email is not valid");
            }
            _email = value;
        }
    }
    private string _password = "";
    public string Password
    {
        get { return _password; }
        set
        {
            if (value.Length < 6)
            {
                throw new ArgumentException("Password must be at least 6 characters long");
            }
            _password = value;
        }
    }
    private string _role = "";
    public string Role
    {
        get { return _role; }
        set
        {
            if (value != "admin" && value != "user")
            {
                throw new ArgumentException("Role must be admin or user");
            }
            _role = value;
        }
    }
}
