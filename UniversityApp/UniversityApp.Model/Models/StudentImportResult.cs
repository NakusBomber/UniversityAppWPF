using UniversityApp.Model.Entities;

namespace UniversityApp.ViewModel.Models;

public class StudentImportResult
{
    public IEnumerable<Student> StudentsWithoutGroup { get; private set; }

    public int CountError { get; private set; }

    public StudentImportResult(IEnumerable<Student> studentsWithoutGroup, int countError)
    {
        StudentsWithoutGroup = studentsWithoutGroup;
        CountError = countError;
    }
}
