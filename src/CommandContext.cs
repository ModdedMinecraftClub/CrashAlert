namespace Mmcc.CrashAlert;

public class CommandContext
{
    public string BaseDir { get; set; } = null!;
    
    public int Interval { get; set; }
    public string ServerRuntimeDir => Path.Join(BaseDir, "server-runtime");
    public string CrashLogsDir => Path.Join(ServerRuntimeDir, "crash-reports");
}