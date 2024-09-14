using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniversityApp.Model.Entities;

[Table("Groups")]
[Index(nameof(Name), IsUnique = true)]
public class Group : Entity
{
    [Key]
    public override Guid Id { get; set; }

    [Required]
    [StringLength(50)]
    public string? Name { get; set; }

    [Required]
    [ForeignKey(nameof(CourseId))]
    public virtual Course? Course { get; set; }

    [Required]
    public Guid? CourseId { get; set; }

    [Required]
    [ForeignKey(nameof(TeacherId))]
    public virtual Teacher? Teacher { get; set; }

    [Required]
    public Guid? TeacherId { get; set; }

    public virtual List<Student> Students { get; set; } = new List<Student>();
    public Group()
    {
        Id = Guid.Empty;
        Name = string.Empty;
        Course = null;
        CourseId = Guid.Empty;
        Teacher = null;
        TeacherId = Guid.Empty;
    }

    public Group(string name, Course course, Teacher teacher)
        : this(Guid.NewGuid(), name, course, teacher)
    {
    }

    public Group(Guid id, string name, Course course, Teacher teacher)
    {
        Id = id;
        Name = name;
        Course = course;
        CourseId = course.Id;
        Teacher = teacher;
        TeacherId = teacher.Id;
    }
    public override int GetHashCode()
    {
        return (Id, Name, CourseId, TeacherId).GetHashCode();
    }

    public bool FullCompare(Group other)
    {
        return this.Id == other.Id &&
            this.Name == other.Name &&
            this.Course == other.Course &&
            this.Teacher == other.Teacher;
    }
}
