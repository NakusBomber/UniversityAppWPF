using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;

namespace UniversityApp.ViewModel.Interfaces;

public interface IValidationViewModel : INotifyDataErrorInfo
{
    public void Validate<TValidationRule>(object? value, [CallerMemberName] string? propertyName = null)
        where TValidationRule : ValidationRule, new();
    public void Validate(object? value, ValidationRule rule, [CallerMemberName] string? propertyName = null);
}
