using UniversityApp.Model.Entities;

namespace UniversityApp.Model.Tests.EntitiesTests;

public class GroupTests
{
    public static IEnumerable<object[]> GetGroupData()
    {
        var course1 = new Course(Guid.NewGuid(), "Course1", "Description1");
        var course2 = new Course(Guid.NewGuid(), "Course2", "Description2");
        var teacher1 = new Teacher(Guid.NewGuid(), "Teacher1", "LastName1");
        var teacher2 = new Teacher(Guid.NewGuid(), "Teacher2", "LastName2");

        var group1 = new Group(Guid.NewGuid(), "Group1", course1, teacher1);
        var group2 = new Group(Guid.NewGuid(), "Group2", course2, teacher2);
        var group1Copy = new Group(group1.Id, "Group1", course1, teacher1);
        var group3 = new Group(Guid.NewGuid(), "Group3", course2, teacher1);

        return new List<object[]>
        {
            new object[] { group1, group2, false },
            new object[] { group1, group1Copy, true },     
            new object[] { group2, group3, false },       
            new object[] { group1, group3, false },    
        };
    }


    [Theory]
    [MemberData(nameof(GetGroupData))]
    public void Entities_Group_FullCompare_Tests(Group first, Group other, bool expected)
    {
        var actual = first.FullCompare(other);
        Assert.Equal(expected, actual);
    }
}
