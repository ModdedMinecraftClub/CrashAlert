using Microsoft.EntityFrameworkCore;
using Mmcc.CrashAlert.Database;

namespace Mmcc.CrashAlert.Services;

public class ProcessedCrashLogsService
{
    private readonly CrashAlertContext _dbContext;

    public ProcessedCrashLogsService(CrashAlertContext dbContext)
        => _dbContext = dbContext;

    public async Task<bool> IsProcessedAlready(string filePath)
        => await _dbContext.ProcessedCrashLogs.AnyAsync(pc => pc.FilePath.Equals(filePath));

    public async Task MarkAsProcessed(string filePath)
    {
        var pcl = new ProcessedCrashLog
        {
            FilePath = filePath,
            TimeProcessed = DateTimeOffset.UtcNow
        };

        await _dbContext.ProcessedCrashLogs.AddAsync(pcl);
        await _dbContext.SaveChangesAsync();
    }
}