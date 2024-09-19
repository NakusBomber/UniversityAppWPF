using MigraDoc.DocumentObjectModel;
using MigraDoc.Rendering;
using System.Diagnostics;
using System.IO;
using System.Reflection.Metadata;
using UniversityApp.Model.Entities;
using UniversityApp.Model.Interfaces;
using Xceed.Words.NET;

namespace UniversityApp.Model.Helpers;

public class Exporter : IExporter
{
    private const char _csvSeparator = ',';
    private const string _fontFamily = "Arial";
    private const double _fontSize = 10;
    private string? _filePath;
    
    public bool IsNeedHeaderline { get; set; }
    public EExportTypes ExportType { get; set; }
    public Exporter()
    {
        IsNeedHeaderline = false;
        ExportType = EExportTypes.CSV;
    }

    public void Export(IEnumerable<Student> students)
    {
        ExportAsync(students).Wait();
    }

    /// <summary>
    /// Process export data in new task
    /// </summary>
    /// <param name="students">Export data</param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public async Task ExportAsync(IEnumerable<Student> students)
    {
        if (string.IsNullOrEmpty(_filePath))
        {
            throw new ArgumentException(nameof(_filePath));
        }
        
        await Task.Run(async () =>
        {
            if (ExportType == EExportTypes.CSV)
            {
                await CSVExportAsync(students, _filePath);
            }
            else if (ExportType == EExportTypes.PDF)
            {
                PDFExport(students, _filePath);
            }
            else
            {
                DOCXExport(students, _filePath);
            }

            OpenExportedDirectoryFile();
        });
    }

    public void SetPath(string path)
    {
        if (string.IsNullOrEmpty(path))
        {
            throw new ArgumentException("Path is null or empty");
        }

        if (!Path.HasExtension(path))
        {
            throw new ArgumentException("Path hasn't extension");
        }

        _filePath = path;
    }

    private async Task CSVExportAsync(IEnumerable<Student> students, string filePath)
    {
        if (string.IsNullOrEmpty(filePath))
        {
            throw new ArgumentException("Path is null or empty");
        }

        using (var file = new StreamWriter(filePath, false, encoding: System.Text.Encoding.UTF8))
        {
            if (IsNeedHeaderline)
            {
                await file.WriteLineAsync("NumberStudent,FirstName,LastName");
            }

            var studentsCount = students.Count();
            for (int i = 0; i < studentsCount; i++)
            {
                var data = new List<string>
                    {
                        (i+1).ToString(),
                        students.ElementAt(i).FirstName ?? "",
                        students.ElementAt(i).LastName ?? "",
                    };
                await file.WriteAsync(string.Join(_csvSeparator, data));
                if (i != studentsCount - 1)
                {
                    await file.WriteAsync(Environment.NewLine);
                }
            }
        }
    }

    private void PDFExport(IEnumerable<Student> students, string filePath)
    {
        if (string.IsNullOrEmpty(filePath))
        {
            throw new ArgumentException("Path is null or empty");
        }

        var document = new MigraDoc.DocumentObjectModel.Document();
        var section = document.AddSection();

        var style = document.Styles[StyleNames.Normal];
        style!.Font.Name = _fontFamily;
        style.Font.Size = Unit.FromPoint(_fontSize);

        var nameNumberedListStyle = "NumberedList";
        var listStyle = document.Styles.AddStyle(nameNumberedListStyle, StyleNames.Normal);
        listStyle.ParagraphFormat.ListInfo.NumberPosition = Unit.FromCentimeter(0.5);
        listStyle.ParagraphFormat.LeftIndent = Unit.FromCentimeter(1.0);
        listStyle.ParagraphFormat.ListInfo.ListType = ListType.NumberList1;
        listStyle.ParagraphFormat.ListInfo.ContinuePreviousList = true;

        var studentsCount = students.Count();
        for (int i = 0; i < studentsCount; i++)
        {
            var student = students.ElementAt(i);
            var paragraph = section.AddParagraph();
            paragraph.Style = nameNumberedListStyle;

            paragraph.AddText(student.FullName);
            paragraph.AddLineBreak();
        }

        var renderer = new PdfDocumentRenderer();
        renderer.Document = document;
        
        renderer.RenderDocument();
        renderer.PdfDocument.Save(filePath);
    }

    private void DOCXExport(IEnumerable<Student> students, string filePath)
    {
        if (string.IsNullOrEmpty(filePath))
        {
            throw new ArgumentException("Path is null or empty");
        }

        using (var docx = DocX.Create(filePath))
        {
            var docxList = docx.AddList(listType: Xceed.Document.NET.ListItemType.Numbered, startNumber: 1);

            var studentsCount = students.Count();
            for (int i = 0; i < studentsCount; i++)
            {
                var student = students.ElementAt(i);
                docx.AddListItem(docxList, student.FullName);
            }
            docx.InsertList(docxList);
            docx.Save();
        }
    }

    private void OpenExportedDirectoryFile()
    {
        if (string.IsNullOrEmpty(_filePath))
        {
            throw new ArgumentException("Path is null or empty");
        }

        if (!Path.Exists(_filePath))
        {
            throw new ArgumentException("Not exists this file");
        }
        
        var directoryPath = Path.GetDirectoryName(_filePath);
        if(directoryPath == null)
        {
            throw new ArgumentNullException(nameof(directoryPath));
        }

        Process.Start("explorer.exe", directoryPath);
    }
}
