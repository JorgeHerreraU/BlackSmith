using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackSmith.Domain.Models;
public class WorkingDay : BaseEntity
{
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public DayOfWeek Day { get; set; }
}
