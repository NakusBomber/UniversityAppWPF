using System.Globalization;
using UniversityApp.ViewModel.Validations;

namespace UniversityApp.ViewModel.Tests.ValidationTests;

public class LengthValidationRuleTests
{
    private const int _minLength = 2;
    private const int _maxLength = 10;
    private readonly CultureInfo _cultureInfo = new CultureInfo("en-US");
    private readonly LengthValidationRule _validation = new LengthValidationRule(_minLength, _maxLength);

    [Fact]
    public void LengthValidationRule_Null_Test()
    {
        var expected = false;
        var actual = _validation.Validate(null, _cultureInfo).IsValid;

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void LengthValidationRule_NormalLength_Test()
    {
        var value = "abcdef";
        var expected = true;
        var actual = _validation.Validate(value, _cultureInfo).IsValid;

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void LengthValidationRule_LessThanMinLength_Test()
    {
        var value = "a";
        var expected = false;
        var actual = _validation.Validate(value, _cultureInfo).IsValid;

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void LengthValidationRule_MoreThanMaxLength_Test()
    {
        var value = "aaabbbcccddd";
        var expected = false;
        var actual = _validation.Validate(value, _cultureInfo).IsValid;

        Assert.Equal(expected, actual);
    }
}
