using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniversityApp.Model.Entities;

[Table("Students")]
[Index(nameof(FirstName), nameof(LastName))]
public class Student : Entity
{
    [Key]
    public override Guid Id { get; set; }

    [Required]
    [StringLength(50)]
    public string? FirstName { get; set; }

    [Required]
    [StringLength(50)]
    public string? LastName { get; set; }

    [NotMapped]
    public string FullName => GetFullName();

    [ForeignKey(nameof(GroupId))]
    public Group? Group { get; set; }

    public Guid? GroupId { get; set; }

    public Student()
    {
        Id = Guid.Empty;
        FirstName = string.Empty;
        LastName = string.Empty;
        Group = null;
        GroupId = Guid.Empty;
    }

    public Student(string firstName, string lastName, Group? group = null)
    {
        Id = Guid.NewGuid();
        FirstName = firstName;
        LastName = lastName;
        Group = group;
        GroupId = group == null ? null : group.Id;
    }
    public override int GetHashCode()
    {
        return (Id, FirstName, LastName, GroupId).GetHashCode();
    }

    private string GetFullName()
    {
        string spacer = FirstName != null && LastName != null 
                        ? " " 
                        : "";
        return $"{FirstName}{spacer}{LastName}";
    }
}
