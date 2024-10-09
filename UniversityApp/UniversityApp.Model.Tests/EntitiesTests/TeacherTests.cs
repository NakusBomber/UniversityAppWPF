using UniversityApp.Model.Entities;

namespace UniversityApp.Model.Tests.EntitiesTests;

public class TeacherTests
{

    public static IEnumerable<object[]> GetTeacherData()
    {
        var teacher1 = new Teacher("Alice", "Johnson");
        var teacher2 = new Teacher("Bob", "Smith");
        var teacher1Copy = new Teacher(teacher1.Id, "Alice", "Johnson");
        var teacherWithDifferentName = new Teacher(teacher1.Id, "Alicia", "Johnson");

        return new List<object[]>
        {
            new object[] { teacher1, teacher2, false },    
            new object[] { teacher1, teacher1Copy, true },         
            new object[] { teacher1, teacherWithDifferentName, false }, 
        };
    }


    [Theory]
    [MemberData(nameof(GetTeacherData))]
    public void Entities_Teacher_FullCompare_Tests(Teacher first, Teacher other, bool expected)
    {
        var actual = first.FullCompare(other);
        Assert.Equal(expected, actual);
    }
}
