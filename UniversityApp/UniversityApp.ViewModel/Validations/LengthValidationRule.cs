using System.Globalization;
using System.Windows.Controls;

namespace UniversityApp.ViewModel.Validations;

public class LengthValidationRule : ValidationRule
{
    public int Min { get; set; }
    public int Max { get; set; }
    public LengthValidationRule(int min, int max)
    {
        Min = min;
        Max = max;
    }
    public override ValidationResult Validate(object value, CultureInfo cultureInfo)
    {
        if(value == null)
        {
            return new ValidationResult(false, "Value mustn't be a null");
        }

        var parseValue = (string)value;

        if (parseValue.Length < Min)
        {
            return new ValidationResult(false, $"Length must be more than {Min}");
        }
        if (parseValue.Length > Max)
        {
            return new ValidationResult(false, $"Length must be less than {Max}");
        }
        return ValidationResult.ValidResult;
    }
}
