using UniversityApp.ViewModel.ViewModels.Dialogs;

namespace UniversityApp.ViewModel.Tests.ViewModels.Dialogs;

public class CourseDialogViewModelTests
{
    private readonly CourseDialogViewModel _vm = new CourseDialogViewModel(string.Empty, () => { });

    [Fact]
    public void CourseDialogViewModel_OkCommand_Test()
    {
        var expected = true;
        _vm.OkCommand.Execute(null);
        var actual = _vm.IsSuccess;

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void CourseDialogViewModel_CancelCommand_Test()
    {
        var expected = false;
        _vm.CancelCommand.Execute(null);
        var actual = _vm.IsSuccess;

        Assert.Equal(expected, actual);
    }
}
