using System.Security.Policy;
using UniversityApp.Model.Entities;
using UniversityApp.Model.Interfaces;

namespace UniversityApp.Model.Tests;

public class UnitOfWorkMock : IUnitOfWork
{
    private IRepository<Course> _courseRepository;
    private IRepository<Group> _groupRepository;
    private IRepository<Student> _studentRepository;
    private IRepository<Teacher> _teacherRepository;
    public IRepository<Course> CourseRepository => _courseRepository;

    public IRepository<Group> GroupRepository => _groupRepository;

    public IRepository<Student> StudentRepository => _studentRepository;

    public IRepository<Teacher> TeacherRepository => _teacherRepository;

    public UnitOfWorkMock(
        FakeRepository<Course> courseRepository,
        FakeRepository<Group> groupRepository,
        FakeRepository<Student> studentRepository,
        FakeRepository<Teacher> teacherRepository)
    {
        _courseRepository = courseRepository;
        _groupRepository = groupRepository;
        _studentRepository = studentRepository;
        _teacherRepository = teacherRepository;
    }

    public UnitOfWorkMock() 
        : this (new FakeRepository<Course>(),
              new FakeRepository<Group>(),
              new FakeRepository<Student>(),
              new FakeRepository<Teacher>())
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
