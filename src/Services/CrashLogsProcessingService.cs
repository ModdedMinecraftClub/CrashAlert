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
        var recent = GetRecentCrashLogs().ToList();

        if (!recent.Any())
        {
            Console.WriteLine("No recent crash logs found. Exiting...");
            return;
        }

        Console.WriteLine($"Found {recent.Count} recent crash log(s)");

        foreach (var recentLog in recent)
        {
            var alreadyProcessed = await _processedCrashLogsService.IsProcessedAlready(recentLog);
        
            // skip if already processed;
            if (alreadyProcessed)
            {
                Console.WriteLine($"{recentLog} already processed. Skipping...");
                continue;
            }
            
            Console.WriteLine($"Notifying about {recentLog}...");

            await _notificationService.Notify(recentLog);
            await _processedCrashLogsService.MarkAsProcessed(recentLog);
            
            Console.WriteLine($"Notified about {recentLog}.");
        }
        
        Console.WriteLine("Done!");
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