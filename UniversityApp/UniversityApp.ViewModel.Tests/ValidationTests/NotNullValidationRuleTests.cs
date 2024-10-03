using System.Globalization;
using UniversityApp.ViewModel.Validations;

namespace UniversityApp.ViewModel.Tests.ValidationTests;

public class NotNullValidationRuleTests
{
    private readonly CultureInfo _culture = new CultureInfo("en-US");
    private readonly NotNullValidationRule _validation = new NotNullValidationRule();

    [Fact]
    public void NotNullValidationRule_Null_Test()
    {
        var expected = false;
        var actual = _validation.Validate(null, _culture).IsValid;

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void NotNullValidationRule_NotNull_Test()
    {
        var value = new object();
        var expected = true;
        var actual = _validation.Validate(value, _culture).IsValid;

        Assert.Equal(expected, actual);
    }
}
