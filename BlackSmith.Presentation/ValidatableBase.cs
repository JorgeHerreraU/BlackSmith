using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using Prism.Mvvm;

namespace BlackSmith.Presentation;

public class ValidatableBase : BindableBase, INotifyDataErrorInfo
{
    private readonly Dictionary<string, List<string?>> _errors = new();

    public bool HasErrors => _errors.Any();

    public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

    public IEnumerable GetErrors(string? propertyName)
    {
        if (propertyName is null) return Enumerable.Empty<string>();
        return _errors.TryGetValue(propertyName, out var list) ? list! : Enumerable.Empty<string>();
    }

    protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
    {
        RaisePropertyChanged(propertyName);
        Validate(propertyName);
    }

    private void OnErrorsChanged(string propertyName)
    {
        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
    }

    private void Validate(string propertyName)
    {
        {
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(this);

            Validator.TryValidateObject(this, validationContext, validationResults, true);

            var result = validationResults.Find(x => x.MemberNames.FirstOrDefault() == propertyName);

            RemoveExistingErrors(validationResults);

            if (result is null) return;

            var key = result.MemberNames.First();

            _errors[key] = new List<string?> { result.ErrorMessage };

            OnErrorsChanged(key);
        }
    }

    private void RemoveExistingErrors(IReadOnlyCollection<ValidationResult> validationResults)
    {
        foreach (var kv in _errors.ToList()
                     .Where(kv => validationResults.All(r => r.MemberNames.All(m => m != kv.Key))))
        {
            _errors.Remove(kv.Key);
            OnErrorsChanged(kv.Key);
        }
    }
}