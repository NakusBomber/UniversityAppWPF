using UniversityApp.Model.Entities;
using UniversityApp.Model.Interfaces;
using UniversityApp.ViewModel.Models;

namespace UniversityApp.Model.Helpers;

public class StudentImporter : IImporter<StudentImportResult>
{
    private const char _csvSeparator = ',';
    private readonly ILineIterator _lineIterator;
    
    public StudentImporter(ILineIterator lineIterator)
    {
        _lineIterator = lineIterator;
    }

    public StudentImporter()
        : this(new LineIterator())
    {
    }

    public StudentImportResult Import(string path)
    {
        return ImportAsync(path).Result;
    }

    public async Task<StudentImportResult> ImportAsync(string path)
    {
        var data = new List<Student>();
        int countError = 0;
        _lineIterator.FilePath = path;
        
        string? line = await _lineIterator.GetNextLineAsync();
        while(line != null)
        {
            try
            {
                if (!IsHeadline(line))
                {
                    var studentInfo = line.Split(_csvSeparator);
                    var firstName = studentInfo[1];
                    var lastName = studentInfo[2];
                    data.Add(new Student(firstName, lastName));
                }
            }
            catch (Exception)
            {
                countError++;
            }
            line = await _lineIterator.GetNextLineAsync();
        }

        return new StudentImportResult(data, countError);
    }

    private bool IsHeadline(string line)
    {
        if (line.Length < 1)
        {
            return false;
        }
        var firstChar = line.First();
        return char.IsLetter(firstChar);
    }
}
