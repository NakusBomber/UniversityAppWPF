using UniversityApp.Model.Entities;

namespace UniversityApp.ViewModel.Models;

public class GroupDialogResult
{
    public bool IsSuccess { get; private set; }
    public Group? Group { get; private set; }

    public GroupDialogResult(bool isSuccess, Group? group = null)
    {
        IsSuccess = isSuccess;
        Group = group;
    }
}
