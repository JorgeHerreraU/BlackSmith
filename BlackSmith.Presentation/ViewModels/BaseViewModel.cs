using System;
using System.ComponentModel;

namespace BlackSmith.Presentation.ViewModels;

public class BaseViewModel : INotifyPropertyChanged, IDisposable
{
    public virtual void Dispose()
    {
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}