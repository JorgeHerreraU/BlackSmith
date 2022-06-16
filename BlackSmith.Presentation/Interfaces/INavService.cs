using System;
using BlackSmith.Presentation.Services;

namespace BlackSmith.Presentation.Interfaces;

public interface INavService
{
    event EventHandler<NavigationTriggeredEventArgs> NavigationTriggered;
    void Navigate(NavigationTriggeredEventArgs value);
}