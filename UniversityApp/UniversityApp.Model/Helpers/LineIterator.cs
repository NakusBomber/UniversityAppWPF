using System.IO;
using UniversityApp.Model.Interfaces;

namespace UniversityApp.Model.Helpers;

public class LineIterator : ILineIterator, IDisposable
{
    private StreamReader? _stream;
    private int _lineNumber = 0;
    public int LineNumber => _lineNumber;

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

            if (!Path.Exists(value))
            {
                throw new ArgumentException("Path not exists");
            }

            _filePath = value;
        }
    }

    public string? GetNextLine()
    {
        return GetNextLineAsync().Result;
    }

    public async Task<string?> GetNextLineAsync()
    {
        if(_stream == null)
        {
            _stream = CreateStream();
        }

        var line = await _stream.ReadLineAsync();
        if(line == null)
        {
            CloseStream();
            return null;
        }

        _lineNumber++;
        return line;
    }

    public void Dispose()
    {
        CloseStream();
    }

    private StreamReader CreateStream()
    {
        if (string.IsNullOrEmpty(FilePath))
        {
            throw new ArgumentException("Path is null or empty");
        }

        _lineNumber = 0;
        return File.OpenText(FilePath);
    }

    private void CloseStream()
    {
        _stream?.Dispose();
        _stream = null;
    }
}
