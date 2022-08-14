using System;
using System.Collections.Generic;

namespace BlackSmith.Presentation.Helpers;
public static class DateHelper
{
    public static IEnumerable<DateTime> GetDateRange(DateTime startDate, DateTime endDate)
    {
        for (var dt = startDate.Date; dt <= endDate.Date; dt = dt.AddDays(1))
        {
            yield return dt;
        }
    }
}
