using UniversityApp.Model.Entities;

namespace UniversityApp.Model.Tests.EntitiesTests;

public class CourseTests
{
    
    public static IEnumerable<object[]> GetListData()
    {
        var course1 = new Course(Guid.NewGuid(), "Name1", "Desc1");
        var courseId1 = new Course(course1.Id, "Name2", "Desc1");
        var course2 = new Course(Guid.NewGuid(), "Name2", "Desc1");
        var course3 = new Course(Guid.NewGuid(), "Name3", "Desc2");
        var course4 = new Course(Guid.NewGuid(), "Name4", "Desc4");
        var course4Copy = new Course(course4.Id, "Name4", "Desc4");
        return new List<object[]>
        {
            new object[] { course1, course2, false },
            new object[] { course2, courseId1, false },
            new object[] { course1, courseId1, false },
            new object[] { course3, course4, false },
            new object[] { course4Copy, course4, true },
        };

    }

    [Theory]
    [MemberData(nameof(GetListData))]
    public void Entities_Course_FullCompare_Tests(Course first, Course other, bool expected)
    {
        var actual = first.FullCompare(other);

        Assert.Equal(expected, actual);
    }
}
