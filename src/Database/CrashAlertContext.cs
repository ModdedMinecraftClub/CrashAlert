using Microsoft.EntityFrameworkCore;

namespace Mmcc.CrashAlert.Database;

public class CrashAlertContext : DbContext
{
    public DbSet<ProcessedCrashLog> ProcessedCrashLogs { get; set; } = null!;

    public string DbPath { get; }

    public CrashAlertContext()
    {
        const Environment.SpecialFolder appData = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(appData);
        DbPath = Path.Join(path, "processed_crash_alerts.db");
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        Console.WriteLine($"Using database: {DbPath}");
        
        options.UseSqlite($"Data Source={DbPath}");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
        => modelBuilder.Entity<ProcessedCrashLog>().HasKey(c => c.FilePath);
}