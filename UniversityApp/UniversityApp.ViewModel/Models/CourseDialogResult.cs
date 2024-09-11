using UniversityApp.Model.Entities;

namespace UniversityApp.ViewModel.Models;

public class CourseDialogResult
{
    public bool IsSuccess { get; private set; }
    public Course? Course { get; private set; }

    public CourseDialogResult(bool isSuccess, Course? course = null)
    {
        IsSuccess = isSuccess;
        Course = course;
    }
}
