namespace MMSL.Server.Core.Import
{
    public class ImportResult
    {
        public int ImportedCount { get; private set; }

        public ImportResult(int importedCount)
        {
            ImportedCount = importedCount;
        }
    }
}
