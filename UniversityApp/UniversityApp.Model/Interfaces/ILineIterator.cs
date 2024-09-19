namespace UniversityApp.Model.Interfaces;

public interface ILineIterator
{
    public int LineNumber { get; }
    public string? GetNextLine();
    public Task<string?> GetNextLineAsync();

}
