using CliFx;
using CliFx.Attributes;
using CliFx.Infrastructure;
using Mmcc.CrashAlert.Services;

namespace Mmcc.CrashAlert.Commands;

[Command]
public class ProcessExitCommand : ICommand
{
    private readonly CommandContext _commandContext;
    private readonly CrashLogsProcessingService _processingService;

    [CommandParameter(0, Description = "The path to the base server directory (i.e. the one which contains run.sh)")]
    public string BaseServerDirectory { get; init; } = null!;
    
    [CommandOption("interval", 'i', Description = "Time interval (in minutes) after which a crash is no longer considered recent.")]
    public int Interval { get; init; } = 10;

    public ProcessExitCommand(CommandContext commandContext, CrashLogsProcessingService processingService)
    {
        _commandContext = commandContext;
        _processingService = processingService;
    }

    public async ValueTask ExecuteAsync(IConsole console)
    {
        _commandContext.BaseDir = BaseServerDirectory;
        _commandContext.Interval = Interval;
        
        await _processingService.ProcessCrashLogs();
    }
}