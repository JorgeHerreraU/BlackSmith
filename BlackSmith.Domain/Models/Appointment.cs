using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackSmith.Domain.Models;
public class Appointment : BaseEntity
{
    public int PatientId { get; set; }
    public int DoctorId { get; set; }
    public Patient Patient { get; set; } = new();
    public Doctor Doctor { get; set; } = new();
    public string Title { get; set; } = "";
    public string Description { get; set; } = "";
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
}
