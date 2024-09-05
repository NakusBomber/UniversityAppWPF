using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniversityApp.Model.Entities;

[Table("Courses")]
[Index(nameof(Name), IsUnique = true)]
public class Course : Entity
{
    [Key]
    public override Guid Id { get; set; }

    [Required]
    [StringLength(75)]
    public string? Name { get; set; }

    [StringLength(300)]
    public string? Description { get; set; }

    public virtual List<Group> Groups { get; set; } = new List<Group>();

    public Course()
    {
        Id = Guid.Empty;
        Name = string.Empty;
        Description = string.Empty;
    }
    public Course(string name, string? description = null)
    {
        Id = Guid.NewGuid();
        Name = name;
        Description = description;
    }

    public override int GetHashCode()
    {
        return (Id, Name, Description).GetHashCode();
    }
}
