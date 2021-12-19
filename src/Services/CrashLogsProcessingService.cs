namespace Mmcc.CrashAlert.Services;

public class CrashLogsProcessingService
{
    private readonly CommandContext _commandContext;
    private readonly ProcessedCrashLogsService _processedCrashLogsService;
    private readonly CrashNotificationService _notificationService;

    public CrashLogsProcessingService(
        CommandContext commandContext,
        ProcessedCrashLogsService processedCrashLogsService,
        CrashNotificationService notificationService
    )
    {
        _commandContext = commandContext;
        _processedCrashLogsService = processedCrashLogsService;
        _notificationService = notificationService;
    }

    public async Task ProcessCrashLogs()
    {
        var last = GetRecentCrashLogs().FirstOrDefault();

        if (last is null)
        {
            Console.WriteLine("No recent crash logs found. Exiting...");
            return;
        }

        Console.WriteLine($"Found a recent crash log - {last}");

        // check if already processed;
        var alreadyProcessed = await _processedCrashLogsService.IsProcessedAlready(last);
        
        if (alreadyProcessed)
        {
            Console.WriteLine("Already processed. Exiting...");
            return;
        }

        await _notificationService.Notify(last);
        await _processedCrashLogsService.MarkAsProcessed(last);
        
        Console.WriteLine("Notified.\n\nDone!");
    }

    private IEnumerable<string> GetRecentCrashLogs()
    {
        var crashLogs = Directory.EnumerateFiles(_commandContext.CrashLogsDir, "*.txt");
        var recent = crashLogs
            .Where(log =>
                File.GetLastWriteTimeUtc(log) > DateTime.UtcNow - TimeSpan.FromMinutes(_commandContext.Interval))
            .OrderByDescending(File.GetLastWriteTimeUtc);

        return recent;
    }
}