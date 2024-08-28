using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniversityApp.Model.Entities;

[Table("Teachers")]
[Index(nameof(FirstName), nameof(LastName))]
public class Teacher : Student
{
    public Teacher() : base()
    {
    }
    public Teacher(string firstName, string lastName, Group? group = null)
        : base(firstName, lastName, group)
    {
        Id = Guid.NewGuid();
        FirstName = firstName;
        LastName = lastName;
        Group = group;
        GroupId = group == null ? null : group.Id;
    }

}
