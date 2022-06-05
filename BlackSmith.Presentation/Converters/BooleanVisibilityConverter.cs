using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace BlackSmith.Presentation.Converters;

public class BooleanVisibilityConverter : IValueConverter
{
    public bool Negate { get; set; }
    public Visibility FalseVisibility { get; set; }

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var result = bool.TryParse(value.ToString(), out var bVal);
        if (!result) return value;
        return bVal switch
        {
            true when !Negate => Visibility.Visible,
            true when Negate => FalseVisibility,
            false when Negate => Visibility.Visible,
            false when !Negate => FalseVisibility,
            _ => Visibility.Visible
        };
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}