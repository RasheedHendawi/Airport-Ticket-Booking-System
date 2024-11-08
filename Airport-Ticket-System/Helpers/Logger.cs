namespace Airport_Ticket_System.Helpers
{
    public static class Logger
    {
        private static readonly string LogFilePath = FilePathHelper.GetDataFilePath("Logger.csv");

        public static void LogError(string message)
        {
            using var writer = new StreamWriter(LogFilePath, true);
            writer.WriteLine($"{DateTime.Now}: {message}");
        }
    }
}