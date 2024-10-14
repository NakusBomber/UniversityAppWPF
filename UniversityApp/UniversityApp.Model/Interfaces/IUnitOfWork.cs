using UniversityApp.Model.Entities;

namespace UniversityApp.Model.Interfaces;

public interface IUnitOfWork
{
    public IRepository<Course> CourseRepository { get; }
    public IRepository<Group> GroupRepository { get; }
    public IRepository<Student> StudentRepository { get; }
    public IRepository<Teacher> TeacherRepository { get; }

    public Task SaveAsync();
    public void Save();
}
