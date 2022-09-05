using BlackSmith.Presentation.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BlackSmith.Presentation.Helpers;

public static class WorkingDayHelper
{
    public static IEnumerable<WorkingDay> GetDefaultWorkingDays()
    {
        return Enum.GetValues(typeof(DayOfWeek))
            .OfType<DayOfWeek>()
            .Select(day => new WorkingDay
            {
                Day = day,
                StartTime = TimeOnly.MinValue,
                EndTime = TimeOnly.MaxValue,
                IsChecked = false
            });
    }
}
