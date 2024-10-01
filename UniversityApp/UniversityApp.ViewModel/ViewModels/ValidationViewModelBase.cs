using System.Collections;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using UniversityApp.ViewModel.Interfaces;

namespace UniversityApp.ViewModel.ViewModels;

public class ValidationViewModelBase : ViewModelBase, IValidationViewModel
{
    private readonly Dictionary<string, List<string>> _errors = new Dictionary<string, List<string>>();
    public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;
    public bool HasErrors => _errors.Any();

    public IEnumerable GetErrors(string? propertyName)
    {
        var result = new List<string>();
        if (string.IsNullOrEmpty(propertyName))
        {
            return result;
        }
        return _errors.TryGetValue(propertyName, out result) ? result : new List<string>();
    }
    public void Validate<TValidationRule>(object? value, [CallerMemberName] string? propertyName = null) 
        where TValidationRule : ValidationRule, new()
    {
        Validate(value, new TValidationRule(), propertyName);
    }

    public void Validate(object? value, ValidationRule rule, [CallerMemberName] string? propertyName = null)
    {
        ClearErrors(propertyName);

        var culture = CultureInfo.DefaultThreadCurrentCulture;
        var result = rule.Validate(value, culture);

        if (result != null && !result.IsValid && result.ErrorContent != null)
        {
            SetError((string)result.ErrorContent, propertyName);
        }
    }

    protected virtual void OnErrorsChanged(string propertyName)
    {
        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
    }
    private void SetError(string error, [CallerMemberName] string? propertyName = null)
    {
        if(propertyName == null)
        {
            throw new ArgumentNullException(nameof(propertyName));
        }

        if (!_errors.ContainsKey(propertyName))
        {
            _errors[propertyName] = new List<string>();
        }

        if (!_errors[propertyName].Contains(error))
        {
            _errors[propertyName].Add(error);
            OnErrorsChanged(propertyName);
        }
    }

    private void ClearErrors([CallerMemberName] string? propertyName = null)
    {
        if (propertyName == null)
        {
            throw new ArgumentNullException(nameof(propertyName));
        }

        if (_errors.Remove(propertyName))
        {
            OnErrorsChanged(propertyName);
        }
    }


}
