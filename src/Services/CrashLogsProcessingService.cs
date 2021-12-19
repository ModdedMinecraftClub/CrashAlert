using Microsoft.EntityFrameworkCore;
using Mmcc.CrashAlert.Database;

namespace Mmcc.CrashAlert.Services;

public class CrashLogsProcessingService
{
    private readonly CommandContext _commandContext;
    private readonly CrashAlertContext _dbContext;

    public CrashLogsProcessingService(CommandContext commandContext, CrashAlertContext dbContext)
    {
        _commandContext = commandContext;
        _dbContext = dbContext;
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
        Console.WriteLine($"Using database: {_dbContext.DbPath}");
        
        // check if already processed;
        var alreadyProcessed = await _dbContext.ProcessedCrashLogs.AnyAsync(pc => pc.FilePath.Equals(last));

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