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


    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected void SetPropertyChanged<T>(ref T member, T val, [CallerMemberName] string propertyName = null!)
    {
        if (Equals(member, val)) return;
        member = val;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}