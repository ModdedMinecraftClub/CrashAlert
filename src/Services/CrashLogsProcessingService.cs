using Microsoft.EntityFrameworkCore;
using Mmcc.CrashAlert.Database;

namespace Mmcc.CrashAlert.Services;

public class CrashLogsProcessingService
{
    private readonly CommandContext _commandContext;
    private readonly ProcessedCrashLogsService _processedCrashLogsService;

    public CrashLogsProcessingService(CommandContext commandContext, ProcessedCrashLogsService processedCrashLogsService)
    {
        _commandContext = commandContext;
        _processedCrashLogsService = processedCrashLogsService;
    }

    private string GetFullCrashLogPath(string logFileName) => Path.Join(_commandContext.CrashLogsDir, logFileName);

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