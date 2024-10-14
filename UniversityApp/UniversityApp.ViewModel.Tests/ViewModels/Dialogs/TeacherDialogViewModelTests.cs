using UniversityApp.ViewModel.ViewModels.Dialogs;

namespace UniversityApp.ViewModel.Tests.ViewModels.Dialogs;

public class TeacherDialogViewModelTests
{
    private readonly TeacherDialogViewModel _vm = new TeacherDialogViewModel(string.Empty, () => { });

    [Fact]
    public void TeacherDialogViewModel_OkCommand_Test()
    {
        var expected = true;
        _vm.OkCommand.Execute(null);
        var actual = _vm.IsSuccess;

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void TeacherDialogViewModel_CancelCommand_Test()
    {
        var expected = false;
        _vm.CancelCommand.Execute(null);
        var actual = _vm.IsSuccess;

        Assert.Equal(expected, actual);
    }
}
