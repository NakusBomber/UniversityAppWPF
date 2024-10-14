using System.Globalization;
using System.Windows.Controls;

namespace UniversityApp.ViewModel.Validations;

public class EntityNameValidationRule : LengthValidationRule
{
    private readonly ValidationResult _errorOnlyLettersResult = new ValidationResult(false, "Allowed only letters");
    public EntityNameValidationRule(int min, int max)
        : base(min, max)
    {
    }

    public override ValidationResult Validate(object value, CultureInfo cultureInfo)
    {
        try
        {
            var result = base.Validate(value, cultureInfo);
            if(result != null && !result.IsValid)
            {
                return result;
            }

            var parseValue = (string)value;

            if (parseValue.StartsWith(' ') || parseValue.EndsWith(' '))
            {
                return new ValidationResult(false, "Must not begin and end with a whitespace");
            }

            if (parseValue.Any(c => !(char.IsLetter(c) || char.IsWhiteSpace(c))))
            {
                return _errorOnlyLettersResult;
            }
        }
        catch (Exception)
        {
            return _errorOnlyLettersResult;
        }

        return ValidationResult.ValidResult;
    }
}
