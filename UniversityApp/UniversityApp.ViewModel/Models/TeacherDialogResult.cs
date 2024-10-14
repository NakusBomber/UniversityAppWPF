using UniversityApp.Model.Entities;

namespace UniversityApp.ViewModel.Models;

public class TeacherDialogResult
{
    public bool IsSuccess { get; private set; }

    public Teacher? Teacher { get; private set; }

    public TeacherDialogResult(bool isSuccess, Teacher? teacher = null)
    {
        IsSuccess = isSuccess;
        Teacher = teacher;
    }
}
