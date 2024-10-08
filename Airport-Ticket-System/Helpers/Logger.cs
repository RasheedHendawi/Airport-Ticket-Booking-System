using System.IO;

public static class Logger
{
    private static readonly string LogFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Logger.csv");

    public static void LogError(string message)
    {
        using (var writer = new StreamWriter(LogFilePath, true))
        {
            writer.WriteLine($"{DateTime.Now}: {message}");
        }
    }
}
