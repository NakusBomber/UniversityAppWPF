using UniversityApp.Model.Entities;
using UniversityApp.Model.Interfaces;
using UniversityApp.Model.Repositories;

namespace UniversityApp.Model.Helpers;

public class UnitOfWork : IUnitOfWork, IDisposable
{
    private readonly ApplicationContext _context = new ApplicationContext();

    private readonly IRepository<Course> _courseRepository;
    private readonly IRepository<Group> _groupRepository;
    private readonly IRepository<Student> _studentRepository;
    private readonly IRepository<Teacher> _teacherRepository;

    public IRepository<Course> CourseRepository => _courseRepository;
    public IRepository<Group> GroupRepository => _groupRepository;
    public IRepository<Student> StudentRepository => _studentRepository;
    public IRepository<Teacher> TeacherRepository => _teacherRepository;

    public UnitOfWork()
    {
        _courseRepository = new GeneralRepository<Course>(_context);
        _groupRepository = new GeneralRepository<Group>(_context);
        _studentRepository = new GeneralRepository<Student>(_context);
        _teacherRepository = new GeneralRepository<Teacher>(_context);
    }

    public void Dispose()
    {
        _context.Dispose();
        GC.SuppressFinalize(this);
    }

    public void Save()
    {
        _context.SaveChanges();
    }

    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }
}
