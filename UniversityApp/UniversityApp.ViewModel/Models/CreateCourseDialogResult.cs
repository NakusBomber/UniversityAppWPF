using UniversityApp.Model.Entities;

namespace UniversityApp.ViewModel.Models;

public class CreateCourseDialogResult
{
    public bool IsSuccess { get; private set; }
    public Course? Course { get; private set; }

    public CreateCourseDialogResult(bool isSuccess, Course? course = null)
    {
        IsSuccess = isSuccess;
        Course = course;
    }
}
