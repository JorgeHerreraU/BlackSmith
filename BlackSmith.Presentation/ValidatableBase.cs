using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;

namespace BlackSmith.Presentation;

public class ValidatableBase : BindableBase, INotifyDataErrorInfo
{
    private readonly Dictionary<string, List<string>?> _errors = new();

    public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged = delegate { };

    public IEnumerable GetErrors(string? propertyName)
    {
        return _errors.ContainsKey(propertyName) ? _errors[propertyName] : null;
    }

    public bool HasErrors => _errors.Count > 0;

    protected override void SetPropertyChanged<T>(ref T member, T val, [CallerMemberName] string propertyName = null!)
    {
        base.SetPropertyChanged(ref member, val, propertyName);

        ValidateProperty(propertyName, val);
    }

    private void ValidateProperty<T>(string propertyName, T value)
    {
        var results = new List<ValidationResult>();
        var context = new ValidationContext(this)
        {
            MemberName = propertyName
        };

        Validator.TryValidateProperty(value, context, results);

        if (results.Any())
            _errors[propertyName] = results.Select(c => c.ErrorMessage).ToList()!;
        else
            _errors.Remove(propertyName);

        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
    }
}