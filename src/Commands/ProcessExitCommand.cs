using CliFx;
using CliFx.Attributes;
using CliFx.Infrastructure;
using Mmcc.CrashAlert.Converters;
using Mmcc.CrashAlert.Services;
using Remora.Rest.Core;

namespace Mmcc.CrashAlert.Commands;

[Command]
public class ProcessExitCommand : ICommand
{
    private readonly CommandContext _commandContext;
    private readonly CrashLogsProcessingService _processingService;

    [CommandParameter(0, Name = "BaseServerDirectory", Description = "The path to the base server directory (i.e. the one which contains run.sh)")]
    public string BaseServerDirectory { get; init; } = null!;
    
    [CommandParameter(1, Name = "WebhookID", Description = "ID of the Discord Webhook to be used for sending crash notifications.", Converter = typeof(SnowflakeConverter))]
    public Snowflake WebhookId { get; init; }

    [CommandParameter(2, Name = "WebhookToken", Description = "Token for the Discord Webhook to be used for sending crash notifications.")]
    public string WebhookToken { get; init; } = null!;
    
    [CommandOption("interval", 'i', Description = "Time interval (in minutes) after which a crash is no longer considered recent.")]
    public int Interval { get; init; } = 10;

    public ProcessExitCommand(CommandContext commandContext, CrashLogsProcessingService processingService)
    {
        _commandContext = commandContext;
        _processingService = processingService;
    }

    public async ValueTask ExecuteAsync(IConsole console)
    {
        SetContext();

        await _processingService.ProcessCrashLogs();
    }

    private void SetContext()
    {
        // have to use the Context to have nice DI;
        _commandContext.BaseDir = BaseServerDirectory;
        _commandContext.Interval = Interval;
        _commandContext.DiscordWebhookId = WebhookId;
        _commandContext.DiscordWebhookToken = WebhookToken;
    }
}