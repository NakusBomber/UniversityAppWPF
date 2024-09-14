using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniversityApp.Model.Entities;

[Table("Teachers")]
[Index(nameof(FirstName), nameof(LastName))]
public class Teacher : Entity
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

    public Teacher()
    {
        Id = Guid.Empty;
        FirstName = string.Empty;
        LastName = string.Empty;
    }

    public Teacher(string firstName, string lastName, Group? group = null)
    {
        Id = Guid.NewGuid();
        FirstName = firstName;
        LastName = lastName;
    }
    public override int GetHashCode()
    {
        return (Id, FirstName, LastName).GetHashCode();
    }

    private string GetFullName()
    {
        string spacer = FirstName != null && LastName != null
                        ? " "
                        : "";
        return $"{FirstName}{spacer}{LastName}";
    }

}
