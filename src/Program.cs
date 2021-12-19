using CliFx;
using Microsoft.Extensions.DependencyInjection;
using Mmcc.CrashAlert;
using Mmcc.CrashAlert.Commands;
using Mmcc.CrashAlert.Converters;
using Mmcc.CrashAlert.Database;
using Mmcc.CrashAlert.Services;
using Remora.Discord.Rest.Extensions;

var services = new ServiceCollection();

services.AddDbContext<CrashAlertContext>();
services.AddDiscordRest(_ => "crashalert");

services.AddScoped<CommandContext>();
services.AddScoped<ProcessedCrashLogsService>();
services.AddScoped<CrashNotificationService>();
services.AddScoped<CrashLogsProcessingService>();

// commands;
services.AddTransient<SnowflakeConverter>();
services.AddTransient<ProcessExitCommand>();

var sp = services.BuildServiceProvider();

return await new CliApplicationBuilder()
    .AddCommandsFromThisAssembly()
    .UseTypeActivator(sp.GetService)
    .Build()
    .RunAsync();