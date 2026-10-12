using System.Diagnostics;
using System.Text;

namespace TaskManagerApp;

public class TaskTraceListener : TraceListener
{
    private StreamWriter? logWriter;
    private bool fileLoggingEnabled;

    public TaskTraceListener(string logPath)
    {
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(logPath)!);
            logWriter = new StreamWriter(logPath, false, new UTF8Encoding(false));
            logWriter.AutoFlush = true;
            fileLoggingEnabled = true;
        }
        catch (Exception exception) when (exception is IOException or UnauthorizedAccessException)
        {
            ReportFileError(exception);
        }
    }

    public override void TraceEvent(TraceEventCache? eventCache, string source,
        TraceEventType eventType, int id, string? message)
    {
        string level = eventType switch
        {
            TraceEventType.Information => "INFO",
            TraceEventType.Warning => "WARN",
            TraceEventType.Error => "ERROR",
            TraceEventType.Critical => "CRITICAL",
            _ => "TRACE"
        };

        WriteLine($"[{level}] {message}");
    }

    public override void Write(string? message)
    {
        Console.Write(message);

        if (!fileLoggingEnabled)
        {
            return;
        }

        try
        {
            logWriter!.Write(message);
        }
        catch (Exception exception) when (exception is IOException or UnauthorizedAccessException)
        {
            fileLoggingEnabled = false;
            ReportFileError(exception);
        }
    }

    public override void WriteLine(string? message)
    {
        message = message?.Replace("\r", "\\r").Replace("\n", "\\n").Replace("\t", "\\t");
        Write($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {message}{Environment.NewLine}");
    }

    public override void Close()
    {
        try
        {
            logWriter?.Dispose();
        }
        catch (Exception exception) when (exception is IOException or UnauthorizedAccessException)
        {
            ReportFileError(exception);
        }

        fileLoggingEnabled = false;
        base.Close();
    }

    private static void ReportFileError(Exception exception)
    {
        Console.WriteLine($"[ERROR] Не удалось записать лог в файл: {exception.Message}");
        Console.WriteLine("Сообщения доступны в консоли");
    }
}
