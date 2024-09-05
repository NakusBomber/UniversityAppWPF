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

    [ForeignKey(nameof(GroupId))]
    public virtual Group? Group { get; set; }

    public Guid? GroupId { get; set; }

    public Teacher()
    {
        Id = Guid.Empty;
        FirstName = string.Empty;
        LastName = string.Empty;
        Group = null;
        GroupId = Guid.Empty;
    }

    public Teacher(string firstName, string lastName, Group? group = null)
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

}
