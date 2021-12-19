using Mmcc.CrashAlert.Database;

namespace Mmcc.CrashAlert.Services;

public interface IProcessedCrashLogsService
{
    Task<bool> IsProcessedAlready(string fileName);
    Task MarkAsProcessed(string fileName);
}

public class ProcessedCrashLogsService : IProcessedCrashLogsService
{
    private readonly CrashAlertContext _dbContext;

    public ProcessedCrashLogsService(CrashAlertContext dbContext)
        => _dbContext = dbContext;

    public async Task<bool> IsProcessedAlready(string filePath)
    {
        throw new NotImplementedException();
    }

    public async Task MarkAsProcessed(string fileName)
    {
        throw new NotImplementedException();
    }
}