using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace BlackSmith.Presentation;

public class BindableBase : INotifyPropertyChanged, IDisposable
{
    public virtual void Dispose()
    {
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void NotifyPropertyChanged([CallerMemberName] string propertyName = null!)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}