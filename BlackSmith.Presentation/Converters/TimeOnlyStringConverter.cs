using System;
using System.Globalization;
using System.Windows.Data;

namespace BlackSmith.Presentation.Converters;

public class TimeOnlyStringConverter : IValueConverter
{
    public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value.ToString();
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return TimeOnly.Parse(value.ToString() ?? string.Empty);
    }
}
