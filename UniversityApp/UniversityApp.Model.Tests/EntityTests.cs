using UniversityApp.Model.Entities;

namespace UniversityApp.Model.Tests;

public class EntityTests
{

    public static IEnumerable<object[]> GetCoursesData()
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

    public static IEnumerable<object[]> GetGroupsData()
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

    public static IEnumerable<object[]> GetStudentsData()
    {
        var group1 = new Group(Guid.NewGuid(), "Group1", new Course(Guid.NewGuid(), "Course1", "Description1"), new Teacher(Guid.NewGuid(), "Teacher1", "LastName1"));
        var group2 = new Group(Guid.NewGuid(), "Group2", new Course(Guid.NewGuid(), "Course2", "Description2"), new Teacher(Guid.NewGuid(), "Teacher2", "LastName2"));

        var student1 = new Student("John", "Doe", group1);
        var student2 = new Student("Jane", "Smith", group2);
        var student1Copy = new Student(student1.Id, "John", "Doe", group1);
        var studentWithoutGroup = new Student("Bob", "Johnson");
        var studentWithoutGroupCopy = new Student(studentWithoutGroup.Id, "Bob", "Johnson");
        var studentWithDifferentGroup = new Student(student1.Id, "John", "Doe", group2);

        return new List<object[]>
        {
            new object[] { student1, student2, false },
            new object[] { student1, student1Copy, true },
            new object[] { student1, studentWithDifferentGroup, false },
            new object[] { student1, studentWithoutGroup, false },
            new object[] { studentWithoutGroup, studentWithoutGroupCopy, true },
        };
    }

    public static IEnumerable<object[]> GetTeachersData()
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
    [MemberData(nameof(GetTeachersData))]
    [MemberData(nameof(GetStudentsData))]
    [MemberData(nameof(GetGroupsData))]
    [MemberData(nameof(GetCoursesData))]
    public void Entity_AreEntityEqual_Tests<T>(T first, T other, bool expected)
    {
        var actual = Entity.AreEntitiesEqual(first, other);

        Assert.Equal(expected, actual);
    }
}
