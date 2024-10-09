using UniversityApp.Model.Interfaces;

namespace UniversityApp.Model.Tests;

public class LineIteratorStub : ILineIterator
{
    private readonly List<string> _lines;
    private int _lineNumber = 0;
    private string? _filePath;
    public string? FilePath
    {
        get => _filePath;
        set
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentException("Path is null or empty");
            }

            _filePath = value;
        }
    }

    public int LineNumber => _lineNumber;

    public LineIteratorStub(IEnumerable<string> lines)
    {
        _lines = lines.ToList();
    }

    public string? GetNextLine()
    {
        if (string.IsNullOrEmpty(FilePath))
        {
            throw new ArgumentException("Path is null or empty");
        }

        var line = _lines.ElementAtOrDefault(_lineNumber);
        _lineNumber++;
        return line?.Trim();
    }

    public async Task<string?> GetNextLineAsync()
    {
        return await Task.Run(GetNextLine);
    }
}
