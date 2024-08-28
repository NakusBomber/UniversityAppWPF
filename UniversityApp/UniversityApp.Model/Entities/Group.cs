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
    public Course? Course { get; set; }

    [Required]
    public Guid? CourseId { get; set; }

    public Group()
    {
        Id = Guid.Empty;
        Name = string.Empty;
        Course = null;
        CourseId = Guid.Empty;
    }

    public Group(string name, Course course)
    {
        Id = Guid.NewGuid();
        Name = name;
        Course = course;
        CourseId = course.Id;
    }
    public override int GetHashCode()
    {
        return (Id, Name, CourseId).GetHashCode();
    }
}
