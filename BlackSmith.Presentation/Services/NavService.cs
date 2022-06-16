using System;
using BlackSmith.Presentation.Enums;
using BlackSmith.Presentation.Interfaces;

namespace BlackSmith.Presentation.Services;

public class NavigationTriggeredEventArgs : EventArgs
{
    public Pages Page { get; set; }
    public object? Model { get; set; }
}

public class NavService : INavService
{
    public event EventHandler<NavigationTriggeredEventArgs>? NavigationTriggered;

    public void Navigate(NavigationTriggeredEventArgs value)
    {
        NavigationTriggered?.Invoke(this, value);
    }
}