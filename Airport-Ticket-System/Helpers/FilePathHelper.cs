namespace Airport_Ticket_System.Helpers
{
    public static class FilePathHelper
    {
        private static string? _testFilePath;

        public static void SetTestFilePath(string path)
        {
            _testFilePath = path;
        }

        public static string GetDataFilePath(string fileName)
        {
            return _testFilePath ?? Path.Combine(Directory.GetCurrentDirectory(), "DataStorage", fileName);
        }
    }

}
