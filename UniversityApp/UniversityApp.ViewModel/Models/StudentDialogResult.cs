using UniversityApp.Model.Entities;

namespace UniversityApp.ViewModel.Models;

public class StudentDialogResult
{
    public bool IsSuccess { get; private set; }

    public Student? Student { get; private set; }

    public StudentDialogResult(bool isSuccess, Student? student = null)
    {
        IsSuccess = isSuccess;
        Student = student;
    }
}
