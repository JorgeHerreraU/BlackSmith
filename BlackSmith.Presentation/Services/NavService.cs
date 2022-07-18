using System;
using BlackSmith.Presentation.Interfaces;

namespace BlackSmith.Presentation.Services;

public class NavigationTriggeredEventArgs : EventArgs
{
    public Type Page { get; set; } = null!;
}

public class NavService : INavService
{
    public event EventHandler<NavigationTriggeredEventArgs>? NavigationTriggered;

    public void Navigate(NavigationTriggeredEventArgs value)
    {
        NavigationTriggered?.Invoke(this, value);
    }
}