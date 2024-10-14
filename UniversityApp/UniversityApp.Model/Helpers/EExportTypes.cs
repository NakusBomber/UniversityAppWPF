namespace UniversityApp.Model.Helpers;

public enum EExportTypes
{
    CSV,
    DOCX,
    PDF
}

public static class EExportTypesExtension
{
    
    public static IEnumerable<EExportTypes> AllTypes
    {
        get => new List<EExportTypes>
        {
            EExportTypes.CSV,
            EExportTypes.DOCX,
            EExportTypes.PDF
        };
    }

    public static string GetFilter(this EExportTypes type)
    {
        switch (type)
        {
            case EExportTypes.CSV:
                return "Comma-separated values (*.csv)|*.csv";
            case EExportTypes.DOCX:
                return "Microsoft Word document (*.docx)|*.docx";
            case EExportTypes.PDF:
                return "Portable document format (*.pdf)|*.pdf";
            
            default:
                throw new NotImplementedException();
        }
    }

}
