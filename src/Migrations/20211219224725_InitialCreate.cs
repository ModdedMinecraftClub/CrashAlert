using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mmcc.CrashAlert.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProcessedCrashLogs",
                columns: table => new
                {
                    FilePath = table.Column<string>(type: "TEXT", nullable: false),
                    TimeProcessed = table.Column<DateTimeOffset>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcessedCrashLogs", x => x.FilePath);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProcessedCrashLogs");
        }
    }
}
