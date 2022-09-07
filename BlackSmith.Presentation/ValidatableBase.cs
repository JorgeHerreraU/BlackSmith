using Prism.Mvvm;
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
    private readonly Dictionary<string, List<string?>> _errors = new();

    public bool HasErrors
    {
        get => _errors.Count > 0;
    }

    public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

    public IEnumerable GetErrors(string? propertyName)
    {
        return propertyName is null
            ? Enumerable.Empty<string>()
            : (IEnumerable)(
                _errors.TryGetValue(propertyName, out var list) ? list! : Enumerable.Empty<string>()
            );
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

            var result = validationResults.Find(
                r => r.MemberNames.FirstOrDefault() == propertyName
            );

            RemoveExistingErrors(validationResults);

            if (result is null) return;

            var key = result.MemberNames.First();

            _errors[key] = new List<string?> { result.ErrorMessage };

            OnErrorsChanged(key);
        }
    }

    private void RemoveExistingErrors(IReadOnlyCollection<ValidationResult> validationResults)
    {
        foreach (
            var keyValue in _errors
                .ToList()
                .Where(
                    kv =>
                        validationResults.All(
                            validationResult =>
                                validationResult.MemberNames.All(member => member != kv.Key)
                        )
                )
        )
        {
            _errors.Remove(keyValue.Key);
            OnErrorsChanged(keyValue.Key);
        }
    }
}
