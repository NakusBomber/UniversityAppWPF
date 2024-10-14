namespace UniversityApp.Model.Interfaces;

public interface ILineIterator
{
    public string? FilePath { get; set; }
    public int LineNumber { get; }
    public string? GetNextLine();
    public Task<string?> GetNextLineAsync();

}
