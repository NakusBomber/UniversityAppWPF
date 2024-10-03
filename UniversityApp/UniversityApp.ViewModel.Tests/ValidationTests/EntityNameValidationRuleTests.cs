using System.Globalization;
using UniversityApp.ViewModel.Validations;

namespace UniversityApp.ViewModel.Tests.ValidationTests;

public class EntityNameValidationRuleTests
{
    private const int _minLength = 1;
    private const int _maxLength = 10;
    private readonly CultureInfo _cultureInfo = new CultureInfo("en-US");
    private readonly EntityNameValidationRule _entityNameValidation = new EntityNameValidationRule(_minLength, _maxLength);

    [Theory]
    [InlineData("", false)]
    [InlineData(" ", false)]
    [InlineData(" dadwa", false)]
    [InlineData("sda ", false)]
    [InlineData("12", false)]
    [InlineData("gs2", false)]
    [InlineData("New entity", true)]
    [InlineData("Entity", true)]
    [InlineData("DsjWjSnZ", true)]
    public void EntityNameValidationRule_Data_Test(string value, bool expected)
    {
        var actual = _entityNameValidation.Validate(value, _cultureInfo).IsValid;

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void EntityNameValidationRule_LengthMoreThanMax_Test()
    {
        var value = new string('a', _maxLength + 1);
        var expected = false;
        var actual = _entityNameValidation.Validate(value, _cultureInfo).IsValid;

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void EntityNameValidationRule_Null_Test()
    {
        var expected = false;
        var actual = _entityNameValidation.Validate(null, _cultureInfo).IsValid;

        Assert.Equal(expected, actual);
    }
}
