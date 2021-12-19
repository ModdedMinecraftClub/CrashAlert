namespace Mmcc.CrashAlert.Database;

public class ProcessedCrashLog
{
    public string FilePath { get; set; } = null!;
    public DateTimeOffset TimeProcessed { get; set; }
}