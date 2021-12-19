using CliFx;
using Microsoft.Extensions.DependencyInjection;
using Mmcc.CrashAlert;
using Mmcc.CrashAlert.Commands;
using Mmcc.CrashAlert.Database;
using Mmcc.CrashAlert.Services;

var services = new ServiceCollection();

services.AddDbContext<CrashAlertContext>();

services.AddScoped<CommandContext>();
services.AddScoped<ProcessedCrashLogsService>();
services.AddScoped<CrashLogsProcessingService>();

// commands;
services.AddTransient<ProcessExitCommand>();

var sp = services.BuildServiceProvider();

return await new CliApplicationBuilder()
    .AddCommandsFromThisAssembly()
    .UseTypeActivator(sp.GetService)
    .Build()
    .RunAsync();