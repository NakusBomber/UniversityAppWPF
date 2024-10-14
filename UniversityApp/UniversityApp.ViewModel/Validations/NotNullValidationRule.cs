using Castle.DynamicProxy.Generators;
using System.Globalization;
using System.Windows.Controls;
using UniversityApp.Model.Entities;

namespace UniversityApp.ViewModel.Validations;

public class NotNullValidationRule : ValidationRule
{
    public override ValidationResult Validate(object value, CultureInfo cultureInfo)
    {
        try
        {
            if (value == null)
            {
                return new ValidationResult(false, "Value must not be a empty");
            }
        }
        catch (Exception e)
        {
            return new ValidationResult(false, $"Error: {e.Message}");
        }

        return ValidationResult.ValidResult;
    }
}
