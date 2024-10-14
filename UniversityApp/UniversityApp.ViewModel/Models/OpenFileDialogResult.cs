namespace UniversityApp.ViewModel.Models;

public class OpenFileDialogResult
{
    public bool IsSuccess { get; private set; }
    public string? FilePath { get; private set; }

    public OpenFileDialogResult(bool isSuccess, string? filePath = null)
    {
        IsSuccess = isSuccess;
        FilePath = filePath;
    }
}
