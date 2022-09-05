using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace BlackSmith.Presentation.Controls;

public class CalendarAttachedProperties : DependencyObject
{
    #region Private Methods

    private static void OnRegisterCommandBindingChanged(
        DependencyObject sender,
        DependencyPropertyChangedEventArgs e
    )
    {
        switch (sender)
        {
            case Calendar calendar:
            {
                if (e.NewValue is ObservableCollection<DateTime> bindings)
                {
                    if (!Calendars.Contains(calendar))
                    {
                        calendar.Tag = bindings;
                        Calendars.Add(calendar);
                    }

                    calendar.BlackoutDates.Clear();
                    foreach (var date in bindings)
                    {
                        calendar.BlackoutDates.Add(new CalendarDateRange(date));
                    }

                    bindings.CollectionChanged += CalendarBindings_CollectionChanged!;
                }
                break;
            }
            case DatePicker datePicker:
            {
                if (e.NewValue is ObservableCollection<DateTime> bindings)
                {
                    if (!DatePickers.Contains(datePicker))
                    {
                        datePicker.Tag = bindings;
                        DatePickers.Add(datePicker);
                    }

                    datePicker.BlackoutDates.Clear();
                    foreach (var date in bindings)
                    {
                        datePicker.BlackoutDates.Add(new CalendarDateRange(date));
                    }

                    bindings.CollectionChanged += DatePickerBindings_CollectionChanged!;
                }
                break;
            }
        }
    }

    #endregion

    #region Attributes

    private static readonly List<Calendar> Calendars = new();
    private static readonly List<DatePicker> DatePickers = new();

    #endregion

    #region Dependency Properties

    public static DependencyProperty RegisterBlackoutDatesProperty =
        DependencyProperty.RegisterAttached(
            "RegisterBlackoutDates",
            typeof(ObservableCollection<DateTime>),
            typeof(CalendarAttachedProperties),
            new PropertyMetadata(null, OnRegisterCommandBindingChanged)
        );

    public static void SetRegisterBlackoutDates(
        DependencyObject d,
        ObservableCollection<DateTime> value
    )
    {
        d.SetValue(RegisterBlackoutDatesProperty, value);
    }

    public static ObservableCollection<DateTime> GetRegisterBlackoutDates(DependencyObject d)
    {
        return (ObservableCollection<DateTime>)d.GetValue(RegisterBlackoutDatesProperty);
    }

    #endregion

    #region Event Handlers

    private static void CalendarBindings_CollectionChanged(
        object sender,
        NotifyCollectionChangedEventArgs e
    )
    {
        var blackoutDates = sender as ObservableCollection<DateTime>;

        var calendar = Calendars.First(c => c.Tag == blackoutDates);

        if (e.Action == NotifyCollectionChangedAction.Add)
        {
            foreach (DateTime date in e.NewItems!)
            {
                calendar.BlackoutDates.Add(new CalendarDateRange(date));
            }
        }

        if (e.Action == NotifyCollectionChangedAction.Remove)
        {
            foreach (DateTime date in e.OldItems!)
            {
                calendar.BlackoutDates.Remove(new CalendarDateRange(date));
            }
        }

        if (e.Action == NotifyCollectionChangedAction.Reset)
        {
            calendar.BlackoutDates.Clear();
        }
    }

    private static void DatePickerBindings_CollectionChanged(
        object sender,
        NotifyCollectionChangedEventArgs e
    )
    {
        var blackoutDates = sender as ObservableCollection<DateTime>;

        var datePicker = DatePickers.First(c => c.Tag == blackoutDates);

        switch (e.Action)
        {
            case NotifyCollectionChangedAction.Add:
            {
                foreach (DateTime date in e.NewItems!)
                {
                    datePicker.BlackoutDates.Add(new CalendarDateRange(date));
                }
                break;
            }
            case NotifyCollectionChangedAction.Remove:
            {
                foreach (DateTime date in e.OldItems!)
                {
                    datePicker.BlackoutDates.Remove(new CalendarDateRange(date));
                }
                break;
            }
            case NotifyCollectionChangedAction.Reset:
                datePicker.BlackoutDates.Clear();
                break;
        }
    }

    #endregion
}
