using UniversityApp.Model.Entities;

namespace UniversityApp.Model.Tests.EntitiesTests;

public class StudentTests
{
    public static IEnumerable<object[]> GetStudentData()
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

    [Theory]
    [MemberData(nameof(GetStudentData))]
    public void Entities_Student_FullCompare_Tests(Student first, Student other, bool expected)
    {
        var actual = first.FullCompare(other);
        Assert.Equal(expected, actual);
    }
}
