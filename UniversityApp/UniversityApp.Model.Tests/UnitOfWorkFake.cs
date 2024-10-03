using System.Security.Policy;
using UniversityApp.Model.Entities;
using UniversityApp.Model.Interfaces;

namespace UniversityApp.Model.Tests;

public class UnitOfWorkFake : IUnitOfWork
{
    private IRepository<Course> _courseRepository;
    private IRepository<Group> _groupRepository;
    private IRepository<Student> _studentRepository;
    private IRepository<Teacher> _teacherRepository;
    public IRepository<Course> CourseRepository => _courseRepository;

    public IRepository<Group> GroupRepository => _groupRepository;

    public IRepository<Student> StudentRepository => _studentRepository;

    public IRepository<Teacher> TeacherRepository => _teacherRepository;

    public UnitOfWorkFake(
        RepositoryFake<Course> courseRepository,
        RepositoryFake<Group> groupRepository,
        RepositoryFake<Student> studentRepository,
        RepositoryFake<Teacher> teacherRepository)
    {
        _courseRepository = courseRepository;
        _groupRepository = groupRepository;
        _studentRepository = studentRepository;
        _teacherRepository = teacherRepository;
    }

    public UnitOfWorkFake() 
        : this (new RepositoryFake<Course>(),
              new RepositoryFake<Group>(),
              new RepositoryFake<Student>(),
              new RepositoryFake<Teacher>())
    {
    }
    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }

    public void Save()
    {
    }

    public async Task SaveAsync()
    {
        await Task.Run(() => { });
    }
}
