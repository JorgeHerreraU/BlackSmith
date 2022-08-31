using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Calendar = System.Windows.Controls.Calendar;

namespace BlackSmith.Presentation.Controls;

public class CalendarAttachedProperties : DependencyObject
{
    #region Attributes

    private static readonly List<Calendar> _calendars = new();
    private static readonly List<DatePicker> _datePickers = new();

    #endregion

    #region Dependency Properties

    public static DependencyProperty RegisterBlackoutDatesProperty = DependencyProperty.RegisterAttached(
        "RegisterBlackoutDates",
        typeof(ObservableCollection<DateTime>),
        typeof(CalendarAttachedProperties),
        new PropertyMetadata(null, OnRegisterCommandBindingChanged));

    public static void SetRegisterBlackoutDates(DependencyObject d, ObservableCollection<DateTime> value)
    {
        d.SetValue(RegisterBlackoutDatesProperty, value);
    }

    public static ObservableCollection<DateTime> GetRegisterBlackoutDates(DependencyObject d)
    {
        return (ObservableCollection<DateTime>)d.GetValue(RegisterBlackoutDatesProperty);
    }

    #endregion

    #region Event Handlers

    private static void CalendarBindings_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        var blackoutDates = sender as ObservableCollection<DateTime>;

        var calendar = _calendars.First(c => c.Tag == blackoutDates);

        if (e.Action == NotifyCollectionChangedAction.Add)
        {
            foreach (DateTime date in e.NewItems)
            {
                calendar.BlackoutDates.Add(new CalendarDateRange(date));
            }
        }

        if (e.Action == NotifyCollectionChangedAction.Remove)
        {
            foreach (DateTime date in e.OldItems)
            {
                calendar.BlackoutDates.Remove(new CalendarDateRange(date));
            }
        }

        if (e.Action == NotifyCollectionChangedAction.Reset)
        {
            calendar.BlackoutDates.Clear();
        }
    }

    private static void DatePickerBindings_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        var blackoutDates = sender as ObservableCollection<DateTime>;

        var datePicker = _datePickers.First(c => c.Tag == blackoutDates);

        if (e.Action == NotifyCollectionChangedAction.Add)
        {
            foreach (DateTime date in e.NewItems)
            {
                datePicker.BlackoutDates.Add(new CalendarDateRange(date));
            }
        }

        if (e.Action == NotifyCollectionChangedAction.Remove)
        {
            foreach (DateTime date in e.OldItems)
            {
                datePicker.BlackoutDates.Remove(new CalendarDateRange(date));
            }
        }

        if (e.Action == NotifyCollectionChangedAction.Reset)
        {
            datePicker.BlackoutDates.Clear();
        }
    }

    #endregion

    #region Private Methods

    private static void OnRegisterCommandBindingChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
    {
        if (sender is Calendar calendar)
        {
            if (e.NewValue is ObservableCollection<DateTime> bindings)
            {
                if (!_calendars.Contains(calendar))
                {
                    calendar.Tag = bindings;
                    _calendars.Add(calendar);
                }

                calendar.BlackoutDates.Clear();
                foreach (var date in bindings)
                {
                    calendar.BlackoutDates.Add(new CalendarDateRange(date));
                }
                bindings.CollectionChanged += CalendarBindings_CollectionChanged;
            }
        }
        else
        {
            if (sender is DatePicker datePicker)
            {
                if (e.NewValue is ObservableCollection<DateTime> bindings)
                {
                    if (!_datePickers.Contains(datePicker))
                    {
                        datePicker.Tag = bindings;
                        _datePickers.Add(datePicker);
                    }

                    datePicker.BlackoutDates.Clear();
                    foreach (var date in bindings)
                    {
                        datePicker.BlackoutDates.Add(new CalendarDateRange(date));
                    }
                    bindings.CollectionChanged += DatePickerBindings_CollectionChanged;
                }
            }
        }
    }

    #endregion
}
