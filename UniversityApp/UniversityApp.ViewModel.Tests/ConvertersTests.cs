using System.Globalization;
using System.Windows;
using UniversityApp.ViewModel.Converters;

namespace UniversityApp.ViewModel.Tests;

public class ConvertersTests
{
    private readonly BooleanToVisibilityConverter _booleanVisibilityConverter = new BooleanToVisibilityConverter();
    private readonly NullToVisibilityConverter _nullVisibilityConverter = new NullToVisibilityConverter();
    private readonly Type _visibilityType = typeof(Visibility);
    private readonly CultureInfo _cultureInfo = new CultureInfo("en-US");
    private object _parameter = new object();
    
    [Fact]
    public void BooleanToVisibilityConverter_Null_Test()
    {
        var expected = Visibility.Collapsed;
        var actual = _booleanVisibilityConverter.Convert(null, _visibilityType, _parameter, _cultureInfo);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void BooleanToVisibilityConverter_False_Test()
    {
        var expected = Visibility.Collapsed;
        var actual = _booleanVisibilityConverter.Convert(false, _visibilityType, _parameter, _cultureInfo);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void BooleanToVisibilityConverter_True_Test()
    {
        var expected = Visibility.Visible;
        var actual = _booleanVisibilityConverter.Convert(true, _visibilityType, _parameter, _cultureInfo);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void BooleanToVisibilityConverter_AnotherType_Test()
    {
        var expected = Visibility.Collapsed;
        var actual = _booleanVisibilityConverter.Convert(1, _visibilityType, _parameter, _cultureInfo);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void NullToVisibilityConverter_Null_Test()
    {
        var expected = Visibility.Collapsed;
        var actual = _nullVisibilityConverter.Convert(null, _visibilityType, _parameter, _cultureInfo);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void NullToVisibilityConverter_Value_Test()
    {
        var expected = Visibility.Visible;
        var actual = _nullVisibilityConverter.Convert(true, _visibilityType, _parameter, _cultureInfo);

        Assert.Equal(expected, actual);
    }
}