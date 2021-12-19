using System.Drawing;
using OneOf;
using Remora.Discord.API.Abstractions.Objects;
using Remora.Discord.API.Abstractions.Rest;
using Remora.Discord.API.Objects;

namespace Mmcc.CrashAlert.Services;

public class CrashNotificationService
{
    private readonly IDiscordRestWebhookAPI _webhookApi;
    private readonly CommandContext _commandContext;

    public CrashNotificationService(IDiscordRestWebhookAPI webhookApi, CommandContext commandContext)
    {
        _webhookApi = webhookApi;
        _commandContext = commandContext;
    }

    public async Task Notify(string filePath)
    {
        var embed = new Embed("Server has crashed", Description: $":no_entry: Server located at `{_commandContext.BaseDir}` has crashed.", Colour: Color.DarkRed);
        await using var fileStream = File.OpenRead(filePath);
        OneOf<FileData, IPartialAttachment> crashLogFile = new FileData(Path.GetFileName(filePath), fileStream);
        var sendNotificationResult = await _webhookApi.ExecuteWebhookAsync(_commandContext.DiscordWebhookId,
            _commandContext.DiscordWebhookToken, embeds: new[] {embed}, attachments: new[] {crashLogFile});

        if (!sendNotificationResult.IsSuccess)
        {
            throw new Exception(
                $"Failed to send the notification due to a Remora error.\nError:${sendNotificationResult.Error}");
        }
    }
}