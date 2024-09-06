using UniversityApp.Model.Entities;
using UniversityApp.Model.Interfaces;
using UniversityApp.Model.Repositories;

namespace UniversityApp.Model.Helpers;

public class UnitOfWork : IUnitOfWork
{
    private ApplicationContext _context = new ApplicationContext();

    private IRepository<Course>? _courseRepository;
    private IRepository<Group>? _groupRepository;
    private IRepository<Student>? _studentRepository;
    private IRepository<Teacher>? _teacherRepository;

    public IRepository<Course> CourseRepository
    {
        get
        {
            if (_courseRepository == null)
            {
                _courseRepository = new GeneralRepository<Course>(_context);
            }
            return _courseRepository;
        }
    }

    public IRepository<Group> GroupRepository
    {
        get
        {
            if (_groupRepository == null)
            {
                _groupRepository = new GeneralRepository<Group>(_context);
            }
            return _groupRepository;
        }
    }

    public IRepository<Student> StudentRepository
    {
        get
        {
            if (_studentRepository == null)
            {
                _studentRepository = new GeneralRepository<Student>(_context);
            }
            return _studentRepository;
        }
    }

    public IRepository<Teacher> TeacherRepository
    {
        get
        {
            if (_teacherRepository == null)
            {
                _teacherRepository = new GeneralRepository<Teacher>(_context);
            }
            return _teacherRepository;
        }
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
